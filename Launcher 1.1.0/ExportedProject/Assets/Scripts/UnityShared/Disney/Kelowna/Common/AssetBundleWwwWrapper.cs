using UnityEngine;
using UnityEngine.Networking;

namespace Disney.Kelowna.Common
{
	public class AssetBundleWwwWrapper : CoroutineReturn
	{
		private const uint CACHE_VERSION_NUMBER = 1u;

		private UnityWebRequest webRequest;

		private AssetBundle cachedAssetBundle;

		private bool isDisposed;

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
				if (cachedAssetBundle == null && webRequest == null)
				{
					return null;
				}
				if (cachedAssetBundle == null && !isDisposed && webRequest.isDone && !webRequest.isError)
				{
					cachedAssetBundle = DownloadHandlerAssetBundle.GetContent(webRequest);
				}
				return cachedAssetBundle;
			}
		}

		public override bool Finished
		{
			get
			{
				if (isDisposed)
				{
					return true;
				}
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

		public void CacheAndDispose()
		{
			cachedAssetBundle = AssetBundle;
			isDisposed = true;
			if (webRequest != null)
			{
				webRequest.Dispose();
			}
		}
	}
}
