using System;
using System.Collections;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	public class WwwBundleDevice : Device
	{
		public const string DEVICE_TYPE = "www-bundle";

		private string baseUri = "";

		private IGcsAccessTokenService gcsAccessTokenService;

		private ICPipeManifestService cpipeManifestService;

		public override string DeviceType
		{
			get
			{
				return "www-bundle";
			}
		}

		public WwwBundleDevice(DeviceManager deviceManager, string baseUri)
			: this(deviceManager, Service.Get<IGcsAccessTokenService>(), Service.Get<ICPipeManifestService>())
		{
			this.baseUri = baseUri;
		}

		public void UpdateBaseUri(string baseUri)
		{
			this.baseUri = baseUri;
		}

		public WwwBundleDevice(DeviceManager deviceManager, IGcsAccessTokenService gcsAccessTokenService, ICPipeManifestService cpipeManifestService)
			: base(deviceManager)
		{
			this.gcsAccessTokenService = gcsAccessTokenService;
			this.cpipeManifestService = cpipeManifestService;
		}

		public override AssetRequest<TAsset> LoadAsync<TAsset>(string deviceList, ref ContentManifest.AssetEntry entry, AssetLoadedHandler<TAsset> handler = null)
		{
			string bundlePath = UriUtil.Combine(baseUri, entry.Key);
			AssetBundleWwwWrapper assetBundleWwwWrapper = new AssetBundleWwwWrapper(bundlePath, gcsAccessTokenService);
			AsyncBundleWwwRequest<TAsset> result = new AsyncBundleWwwRequest<TAsset>(entry.Key, assetBundleWwwWrapper);
			CoroutineRunner.StartPersistent(waitForBundleToLoad(assetBundleWwwWrapper, handler, entry.IsCacheOnly), this, "waitForBundleToLoad");
			return result;
		}

		private IEnumerator waitForBundleToLoad<TAsset>(AssetBundleWwwWrapper bundleRequest, AssetLoadedHandler<TAsset> handler, bool cacheOnly) where TAsset : class
		{
			CPipeManifestResponse cpipeManifestResponse = new CPipeManifestResponse();
			yield return cpipeManifestService.LookupAssetUrl(cpipeManifestResponse, bundleRequest.BundlePath);
			if (string.IsNullOrEmpty(cpipeManifestResponse.FullAssetUrl))
			{
				throw new Exception(string.Format("Bundle \"{0}\" NOT FOUND in CPipe manifest.", bundleRequest.BundlePath));
			}
			while (!Caching.ready)
			{
				yield return null;
			}
			bundleRequest.LoadFromCacheOrDownload(cpipeManifestResponse.FullAssetUrl);
			Service.Get<LoadingController>().RegisterDownload(bundleRequest.WebRequest);
			yield return bundleRequest.Send();
			Service.Get<LoadingController>().UnRegisterDownload(bundleRequest.WebRequest);
			yield return bundleRequest;
			yield return null;
			if (bundleRequest.WebRequest.isError)
			{
				Log.LogErrorFormatted(this, "Failed to download bundle {0} with error: {1}", bundleRequest.BundlePath, bundleRequest.WebRequest.error);
				if (handler != null)
				{
					handler(bundleRequest.BundlePath, null);
				}
			}
			else if (handler != null)
			{
				AssetBundle assetBundle = null;
				if (!cacheOnly)
				{
					assetBundle = bundleRequest.AssetBundle;
				}
				TAsset asset = null;
				if (assetBundle != null)
				{
					asset = (TAsset)(object)assetBundle;
				}
				handler(bundleRequest.BundlePath, asset);
			}
			bundleRequest.CacheAndDispose();
		}

		public override TAsset LoadImmediate<TAsset>(string deviceList, ref ContentManifest.AssetEntry entry)
		{
			throw new InvalidOperationException("Remote asset bundles must be loaded asynchronously.");
		}
	}
}
