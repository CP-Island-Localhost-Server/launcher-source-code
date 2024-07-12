using System;
using System.IO;
using Disney.LaunchPadFramework;
using UnityEngine.Networking;

namespace Disney.Kelowna.Common
{
	internal class CdnDownloadHandlerScript : DownloadHandlerScript
	{
		private int contentLength = -1;

		private long bytesReceived = 0L;

		private readonly FileStream downloadFileStream;

		private readonly string downloadFilename;

		private bool cancelled = false;

		public float Progress { get; private set; }

		public CdnDownloadHandlerScript(FileStream downloadFileStream, string downloadFilename)
		{
			this.downloadFileStream = downloadFileStream;
			this.downloadFilename = downloadFilename;
		}

		public CdnDownloadHandlerScript(FileStream downloadFileStream, string downloadFilename, byte[] buffer)
			: base(buffer)
		{
			this.downloadFileStream = downloadFileStream;
			this.downloadFilename = downloadFilename;
		}

		public void Cancel()
		{
			cancelled = true;
		}

		protected override byte[] GetData()
		{
			return null;
		}

		protected override void ReceiveContentLength(int contentLength)
		{
			this.contentLength = contentLength;
		}

		protected override bool ReceiveData(byte[] data, int dataLength)
		{
			if (cancelled)
			{
				return false;
			}
			if (data == null || data.Length < 1)
			{
				Log.LogError(this, "CdnDownloadHandlerScript - ReceiveData() received a null/empty buffer!");
				return false;
			}
			if (downloadFileStream != null && downloadFileStream.SafeFileHandle != null && downloadFileStream.SafeFileHandle.IsClosed)
			{
				Log.LogError(this, "CdnDownloadHandlerScript - downloadFileStream is Closed!");
				return false;
			}
			try
			{
				if (downloadFileStream != null)
				{
					downloadFileStream.Write(data, 0, dataLength);
				}
				else if (downloadFilename != null)
				{
					appendAllBytes(downloadFilename, data, dataLength);
				}
				bytesReceived += dataLength;
				if (contentLength > 0)
				{
					float num = (float)bytesReceived / (float)contentLength;
					if (num <= 0f)
					{
						Progress = 0f;
					}
					else if (num >= 1f)
					{
						Progress = 1f;
					}
					else
					{
						Progress = num;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return true;
		}

		private static void appendAllBytes(string path, byte[] bytes, int length)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Append))
			{
				fileStream.Write(bytes, 0, length);
			}
		}

		protected override float GetProgress()
		{
			return Progress;
		}

		protected override void CompleteContent()
		{
		}
	}
}
