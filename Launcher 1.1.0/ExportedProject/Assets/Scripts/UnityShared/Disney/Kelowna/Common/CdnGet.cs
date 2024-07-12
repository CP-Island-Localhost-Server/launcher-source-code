using System;
using System.Collections;
using System.IO;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine.Networking;

namespace Disney.Kelowna.Common
{
	public class CdnGet : IDisposable
	{
		public delegate void CdnGetStringComplete(bool success, string payload, string errorMessage);

		public delegate void CdnGetFileComplete(bool success, string filename, string errorMessage);

		private enum ModeEnum
		{
			GetString = 0,
			GetFile = 1
		}

		private int timeoutSeconds = 0;

		private readonly string contentPath;

		private readonly string saveToFilename;

		private CdnGetStringComplete onGetStringComplete;

		private CdnGetFileComplete onGetFileComplete;

		private readonly ModeEnum mode;

		private ICoroutine webRequestCoroutine;

		private CdnDownloadHandlerScript cdnDownloadHandlerScript;

		private FileStream downloadFileStream;

		private byte[] downloadBuffer;

		private float progress = 0f;

		private bool disposed = false;

		public int TimeoutSeconds
		{
			get
			{
				return timeoutSeconds;
			}
			set
			{
				if (webRequestCoroutine == null)
				{
					timeoutSeconds = value;
				}
			}
		}

		public CdnGet(string contentPath, CdnGetStringComplete onGetStringComplete)
		{
			mode = ModeEnum.GetString;
			this.contentPath = contentPath;
			this.onGetStringComplete = onGetStringComplete;
			saveToFilename = null;
			onGetFileComplete = null;
		}

		public CdnGet(string contentPath, string saveToFilename, CdnGetFileComplete onGetFileComplete)
		{
			mode = ModeEnum.GetFile;
			this.contentPath = contentPath;
			onGetStringComplete = null;
			this.saveToFilename = saveToFilename;
			this.onGetFileComplete = onGetFileComplete;
			downloadBuffer = new byte[65536];
		}

		~CdnGet()
		{
			Dispose(false);
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
				if (cdnDownloadHandlerScript != null)
				{
					cdnDownloadHandlerScript.Cancel();
					cdnDownloadHandlerScript = null;
				}
				if (webRequestCoroutine != null && !webRequestCoroutine.Disposed)
				{
					webRequestCoroutine.Stop();
					webRequestCoroutine = null;
				}
				closeDownloadFile();
			}
			downloadBuffer = null;
			onGetStringComplete = null;
			onGetFileComplete = null;
			disposed = true;
		}

		public void Execute()
		{
			if (webRequestCoroutine != null)
			{
				string message = "'CdnGet' objects can only be used once!";
				throw new Exception(message);
			}
			if (mode == ModeEnum.GetFile)
			{
				openDownloadFile();
			}
			webRequestCoroutine = CoroutineRunner.Start(get(), this, "CdnGet");
			webRequestCoroutine.ECancelled += cleanup;
			webRequestCoroutine.ECompleted += cleanup;
		}

		public float GetProgress()
		{
			if (cdnDownloadHandlerScript != null)
			{
				progress = cdnDownloadHandlerScript.Progress;
			}
			return progress;
		}

		public void Cancel()
		{
			if (cdnDownloadHandlerScript != null)
			{
				cdnDownloadHandlerScript.Cancel();
			}
			if (webRequestCoroutine != null && !webRequestCoroutine.Completed && !webRequestCoroutine.Cancelled)
			{
				webRequestCoroutine.Cancel();
			}
		}

		private void cleanup()
		{
			if (webRequestCoroutine != null)
			{
				webRequestCoroutine.ECancelled -= cleanup;
				webRequestCoroutine.ECompleted -= cleanup;
			}
			closeDownloadFile();
		}

		private IEnumerator get()
		{
			ICPipeManifestService cpipeManifestService = Service.Get<ICPipeManifestService>();
			CPipeManifestResponse cpipeManifestResponse = new CPipeManifestResponse();
			yield return cpipeManifestService.LookupAssetUrl(cpipeManifestResponse, contentPath);
			if (string.IsNullOrEmpty(cpipeManifestResponse.FullAssetUrl))
			{
				Log.LogErrorFormatted(this, "ERROR: CdnGet - contentPath '{0}' NOT FOUND!", contentPath);
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
			unityWebRequest.timeout = timeoutSeconds;
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
			UnityWebRequest unityWebRequest = new UnityWebRequest(fullAssetUrl);
			if (downloadBuffer == null || downloadBuffer.Length <= 0)
			{
				cdnDownloadHandlerScript = new CdnDownloadHandlerScript(downloadFileStream, saveToFilename);
			}
			else
			{
				cdnDownloadHandlerScript = new CdnDownloadHandlerScript(downloadFileStream, saveToFilename, downloadBuffer);
			}
			unityWebRequest.downloadHandler = cdnDownloadHandlerScript;
			unityWebRequest.method = "GET";
			unityWebRequest.timeout = timeoutSeconds;
			yield return unityWebRequest.Send();
			cdnDownloadHandlerScript = null;
			closeDownloadFile();
			if (unityWebRequest.isError)
			{
				Log.LogErrorFormatted(this, "Error: getFile({0}):\n{1}", fullAssetUrl, unityWebRequest.error);
				if (onGetFileComplete != null)
				{
					onGetFileComplete(false, saveToFilename, unityWebRequest.error);
				}
			}
			else if (onGetFileComplete != null)
			{
				onGetFileComplete(true, saveToFilename, null);
			}
			onGetFileComplete = null;
			unityWebRequest.Dispose();
		}

		private void openDownloadFile()
		{
			closeDownloadFile();
			File.Delete(saveToFilename);
			downloadFileStream = null;
		}

		private void closeDownloadFile()
		{
			if (downloadFileStream != null)
			{
				downloadFileStream.Dispose();
				downloadFileStream = null;
			}
		}
	}
}
