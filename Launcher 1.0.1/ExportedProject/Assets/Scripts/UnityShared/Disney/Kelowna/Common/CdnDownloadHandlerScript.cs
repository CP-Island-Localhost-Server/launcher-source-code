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

		private bool cancelled = false;

		public float Progress { get; private set; }

		public CdnDownloadHandlerScript(FileStream downloadFileStream)
		{
			this.downloadFileStream = downloadFileStream;
		}

		public CdnDownloadHandlerScript(FileStream downloadFileStream, byte[] buffer)
			: base(buffer)
		{
			this.downloadFileStream = downloadFileStream;
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
			downloadFileStream.Write(data, 0, dataLength);
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
			return true;
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
