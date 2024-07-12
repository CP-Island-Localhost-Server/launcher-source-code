using UnityEngine;
using UnityEngine.Networking;

namespace Disney.Kelowna.Common
{
	public class AssetBundleWwwWrapper : CoroutineReturn
	{
		private const uint CACHE_VERSION_NUMBER = 1u;

		private UnityWebRequest webRequest;

		private string bundlePath;

		private IGcsAccessTokenService gcsAccessTokenService;

		internal UnityWebRequest WebRequest
		{
			get
			{
				return webRequest;
			}
		}

		public string BundlePath
		{
			get
			{
				return bundlePath;
			}
		}

		public IGcsAccessTokenService GcsAccessTokenService
		{
			get
			{
				return gcsAccessTokenService;
			}
		}

		public AssetBundle AssetBundle
		{
			get
			{
				if (webRequest == null)
				{
					return null;
				}
				return ((DownloadHandlerAssetBundle)webRequest.downloadHandler).assetBundle;
			}
		}

		public override bool Finished
		{
			get
			{
				if (webRequest == null)
				{
					return false;
				}
				return webRequest.isDone;
			}
		}

		public AssetBundleWwwWrapper(string bundlePath, IGcsAccessTokenService gcsAccessTokenService)
		{
			this.bundlePath = bundlePath;
			this.gcsAccessTokenService = gcsAccessTokenService;
		}

		public void LoadFromCacheOrDownload(string url)
		{
			webRequest = UnityWebRequest.GetAssetBundle(url, 1u, 0u);
		}

		public void LoadFromDownload(string url)
		{
			webRequest = UnityWebRequest.GetAssetBundle(url);
		}

		public AsyncOperation Send()
		{
			return webRequest.Send();
		}
	}
}
