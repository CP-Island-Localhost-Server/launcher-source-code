using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine.Networking;

namespace Disney.Kelowna.Common
{
	public class CdnGet2 : IDisposable
	{
		public delegate void CdnGetStringComplete(bool success, string payload, string errorMessage);

		public delegate void CdnGetFileComplete(bool success, string filename, string errorMessage);

		private enum ModeEnum
		{
			GetString = 0,
			GetFile = 1
		}

		private readonly string contentPath;

		private readonly string saveToFilename;

		private CdnGetStringComplete onGetStringComplete;

		private CdnGetFileComplete onGetFileComplete;

		private readonly ModeEnum mode;

		private ICoroutine webRequestCoroutine;

		private WebClient webClient;

		private readonly Queue<string> messageQueue = new Queue<string>();

		private volatile bool isMessageQueueEmpty = false;

		private volatile float progress = 0f;

		private volatile bool isWebClientCancelled = false;

		private volatile Exception webClientError;

		private bool disposed = false;

		public int TimeoutSeconds;

		public CdnGet2(string contentPath, CdnGetStringComplete onGetStringComplete)
		{
			mode = ModeEnum.GetString;
			this.contentPath = contentPath;
			this.onGetStringComplete = onGetStringComplete;
			saveToFilename = null;
			onGetFileComplete = null;
		}

		public CdnGet2(string contentPath, string saveToFilename, CdnGetFileComplete onGetFileComplete)
		{
			mode = ModeEnum.GetFile;
			this.contentPath = contentPath;
			onGetStringComplete = null;
			this.saveToFilename = saveToFilename;
			this.onGetFileComplete = onGetFileComplete;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}
			if (disposing)
			{
				if (webClient != null)
				{
					webClient.CancelAsync();
					webClient.Dispose();
				}
				webClient = null;
			}
			if (webRequestCoroutine != null && !webRequestCoroutine.Disposed)
			{
				webRequestCoroutine.Stop();
			}
			webRequestCoroutine = null;
			onGetStringComplete = null;
			onGetFileComplete = null;
			disposed = true;
		}

		public void Execute()
		{
			if (webRequestCoroutine != null)
			{
				string message = "'CdnGet2' objects can only be used once!";
				throw new Exception(message);
			}
			webRequestCoroutine = CoroutineRunner.Start(get(), this, "CdnGet2");
			webRequestCoroutine.ECancelled += cleanup;
			webRequestCoroutine.ECompleted += cleanup;
		}

		public float GetProgress()
		{
			return progress;
		}

		public void Cancel()
		{
			if (webClient != null)
			{
				webClient.CancelAsync();
			}
			if (webRequestCoroutine != null && !webRequestCoroutine.Completed && !webRequestCoroutine.Cancelled)
			{
				webRequestCoroutine.Cancel();
			}
		}

		public string DequeueMessage()
		{
			if (isMessageQueueEmpty)
			{
				return null;
			}
			string result = null;
			lock (messageQueue)
			{
				if (messageQueue.Count > 0)
				{
					result = messageQueue.Dequeue();
				}
				isMessageQueueEmpty = messageQueue.Count < 1;
			}
			return result;
		}

		private void enqueueBackgroundMessage(string msg)
		{
			lock (messageQueue)
			{
				messageQueue.Enqueue(msg);
				isMessageQueueEmpty = false;
			}
		}

		private void cleanup()
		{
			if (webRequestCoroutine != null)
			{
				webRequestCoroutine.ECancelled -= cleanup;
				webRequestCoroutine.ECompleted -= cleanup;
			}
		}

		private IEnumerator get()
		{
			ICPipeManifestService cpipeManifestService = Service.Get<ICPipeManifestService>();
			CPipeManifestResponse cpipeManifestResponse = new CPipeManifestResponse();
			yield return cpipeManifestService.LookupAssetUrl(cpipeManifestResponse, contentPath);
			if (string.IsNullOrEmpty(cpipeManifestResponse.FullAssetUrl))
			{
				Log.LogErrorFormatted(this, "ERROR: CdnGet2 - contentPath '{0}' NOT FOUND!", contentPath);
				yield break;
			}
			switch (mode)
			{
			case ModeEnum.GetString:
				yield return getString(cpipeManifestResponse.FullAssetUrl);
				break;
			case ModeEnum.GetFile:
				yield return getFile(cpipeManifestResponse.FullAssetUrl);
				break;
			}
		}

		private IEnumerator getString(string fullAssetUrl)
		{
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(fullAssetUrl);
			unityWebRequest.timeout = 0;
			unityWebRequest.disposeDownloadHandlerOnDispose = true;
			yield return unityWebRequest.Send();
			if (unityWebRequest.isError)
			{
				Log.LogErrorFormatted(this, "Error: getString({0}):\n{1}", fullAssetUrl, unityWebRequest.error);
				if (onGetStringComplete != null)
				{
					onGetStringComplete(false, null, unityWebRequest.error);
				}
			}
			else if (onGetStringComplete != null)
			{
				onGetStringComplete(true, unityWebRequest.downloadHandler.text, null);
			}
			onGetStringComplete = null;
			unityWebRequest.Dispose();
		}

		private IEnumerator getFile(string fullAssetUrl)
		{
			webClient = new WebClient();
			webClient.DownloadFileCompleted += downloadFileCompletedCallback;
			webClient.DownloadProgressChanged += downloadProgressCallback;
			progress = 0f;
			isWebClientCancelled = false;
			webClientError = null;
			Uri uri = new Uri(fullAssetUrl);
			webClient.DownloadFileAsync(uri, saveToFilename);
			while (webClient.IsBusy)
			{
				yield return null;
			}
			if (webClientError != null)
			{
				Log.LogErrorFormatted(this, "Error: getFile({0}):\n{1}", fullAssetUrl, webClientError.Message);
				if (onGetFileComplete != null)
				{
					onGetFileComplete(false, saveToFilename, webClientError.Message);
				}
			}
			else if (isWebClientCancelled)
			{
				if (onGetFileComplete != null)
				{
					onGetFileComplete(false, saveToFilename, "Cancelled.");
				}
			}
			else if (onGetFileComplete != null)
			{
				onGetFileComplete(true, saveToFilename, null);
			}
		}

		private void downloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
		{
			progress = (float)e.ProgressPercentage / 100f;
		}

		private void downloadFileCompletedCallback(object sender, AsyncCompletedEventArgs e)
		{
			isWebClientCancelled = e.Cancelled;
			webClientError = e.Error;
			string msg = string.Format("{0}  download completed - cancelled={1}, error='{2}'", (string)e.UserState, e.Cancelled, (e.Error == null) ? "NULL" : e.Error.Message);
			enqueueBackgroundMessage(msg);
		}
	}
}
