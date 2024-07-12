using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	public class BundleDevice : Device
	{
		public const string DEVICE_TYPE = "bundle";

		private readonly BundleManager bundleManager;

		private readonly Dictionary<string, AssetRequest<AssetBundle>> activeMountRequests;

		private static int activeRequestCount;

		public override string DeviceType
		{
			get
			{
				return "bundle";
			}
		}

		public BundleDevice(DeviceManager deviceManager, BundleManager bundleManager)
			: base(deviceManager)
		{
			this.bundleManager = bundleManager;
			activeMountRequests = new Dictionary<string, AssetRequest<AssetBundle>>();
		}

		public override AssetRequest<TAsset> LoadAsync<TAsset>(string deviceList, ref ContentManifest.AssetEntry entry, AssetLoadedHandler<TAsset> handler = null)
		{
			if (bundleManager.IsUnmounting(entry.BundleKey))
			{
				bundleManager.CancelUnmount(entry.BundleKey);
			}
			AssetRequest<TAsset> assetRequest;
			if (bundleManager.IsMounted(entry.BundleKey))
			{
				BundleMount bundle = bundleManager.GetBundle(entry.BundleKey);
				if (entry.Extension.Equals("unity"))
				{
					assetRequest = new SceneAssetBundleRequest<TAsset>(entry.Key, entry.BundleKey, null);
					assetRequest.Finished = true;
				}
				else
				{
					assetRequest = bundle.LoadAsync(entry.Key, entry.AssetPath, handler);
				}
			}
			else
			{
				AsyncAssetBundleRequest<TAsset> asyncAssetBundleRequest = ((!entry.Extension.Equals("unity")) ? new AsyncAssetBundleRequest<TAsset>(entry.Key, null) : new SceneAssetBundleRequest<TAsset>(entry.Key, entry.BundleKey, null));
				assetRequest = asyncAssetBundleRequest;
				CoroutineRunner.StartPersistent(loadBundleAndDependenciesAsync(deviceList, entry, asyncAssetBundleRequest, handler), this, "loadBundleAndDependenciesAsync");
			}
			return assetRequest;
		}

		private IEnumerator loadBundleAndDependenciesAsync<TAsset>(string deviceList, ContentManifest.AssetEntry entry, AsyncAssetBundleRequest<TAsset> assetRequest, AssetLoadedHandler<TAsset> handler = null) where TAsset : class
		{
			List<AssetRequest<AssetBundle>> bundleRequests = new List<AssetRequest<AssetBundle>>();
			List<string> bundlesToLoad = new List<string> { entry.BundleKey };
			bundlesToLoad.AddRange(bundleManager.GetDependencyKeys(entry.BundleKey));
			for (int i = 0; i < bundlesToLoad.Count; i++)
			{
				string bundleKey = bundlesToLoad[i];
				if (bundleManager.IsMounted(bundleKey))
				{
					continue;
				}
				AssetRequest<AssetBundle> bundleRequest;
				if (!activeMountRequests.TryGetValue(bundleKey, out bundleRequest))
				{
					ContentManifest.AssetEntry entry2 = createBundleEntry(bundleKey, deviceList);
					activeRequestCount++;
					bundleRequest = DeviceManager.LoadAsync<AssetBundle>(deviceList, ref entry2, delegate
					{
						activeRequestCount--;
					});
					activeMountRequests[bundleKey] = bundleRequest;
				}
				bundleRequests.Add(bundleRequest);
				while (activeRequestCount >= 6)
				{
					yield return null;
				}
			}
			yield return waitForBundlesToLoad<TAsset>(pinBundles: deviceList.Contains("sa-bundle"), key: entry.Key, assetPath: entry.AssetPath, bundleRequests: bundleRequests, assetRequest: assetRequest, handler: handler);
		}

		private IEnumerator waitForBundlesToLoad<TAsset>(string key, string assetPath, List<AssetRequest<AssetBundle>> bundleRequests, bool pinBundles, AsyncAssetBundleRequest<TAsset> assetRequest, AssetLoadedHandler<TAsset> handler) where TAsset : class
		{
			yield return new CompositeCoroutineReturn(bundleRequests.ToArray());
			AssetRequest<AssetBundle> assetRequest3 = bundleRequests[0];
			BundleMount[] bundleMounts = new BundleMount[bundleRequests.Count];
			for (int i = 0; i < bundleRequests.Count; i++)
			{
				AssetRequest<AssetBundle> assetRequest2 = bundleRequests[i];
				if (!bundleManager.IsMounted(assetRequest2.Key))
				{
					BundleMount bundleMount = bundleManager.MountBundle(assetRequest2.Key, assetRequest2.Asset, pinBundles);
					bundleMounts[i] = bundleMount;
				}
				else
				{
					bundleMounts[i] = bundleManager.GetBundle(assetRequest2.Key);
				}
				activeMountRequests.Remove(assetRequest2.Key);
			}
			BundleMount rootMount = bundleMounts[0];
			if (!assetPath.EndsWith(".unity"))
			{
				assetRequest.Request = rootMount.LoadAsync<TAsset>(key, assetPath).Request;
				yield return assetRequest;
			}
			else
			{
				rootMount.IsPinned = true;
				assetRequest.Finished = true;
			}
			if (handler != null)
			{
				handler(key, assetRequest.Asset);
			}
			for (int i = 0; i < bundleRequests.Count; i++)
			{
				AssetRequest<AssetBundle> assetRequest2 = bundleRequests[i];
				assetRequest2.Dispose();
			}
		}

		public override TAsset LoadImmediate<TAsset>(string deviceList, ref ContentManifest.AssetEntry entry)
		{
			if (bundleManager.IsMounted(entry.BundleKey))
			{
				return bundleManager.GetBundle(entry.BundleKey).LoadImmediate<TAsset>(entry.Key, entry.AssetPath);
			}
			List<string> list = new List<string>();
			list.Add(entry.BundleKey);
			List<string> list2 = list;
			list2.AddRange(bundleManager.GetDependencyKeys(entry.BundleKey));
			BundleMount[] array = new BundleMount[list2.Count];
			for (int i = 0; i < list2.Count; i++)
			{
				string bundleKey = list2[i];
				ContentManifest.AssetEntry entry2 = createBundleEntry(bundleKey, deviceList);
				if (!bundleManager.IsMounted(bundleKey))
				{
					AssetBundle bundle = DeviceManager.LoadImmediate<AssetBundle>(deviceList, ref entry2);
					BundleMount bundleMount = bundleManager.MountBundle(bundleKey, bundle, deviceList.Contains("sa-bundle"));
					array[i] = bundleMount;
				}
				else
				{
					array[i] = bundleManager.GetBundle(bundleKey);
				}
			}
			BundleMount bundleMount2 = array[0];
			return bundleMount2.LoadImmediate<TAsset>(entry.Key, entry.AssetPath);
		}

		private static ContentManifest.AssetEntry createBundleEntry(string bundleKey, string deviceList)
		{
			return new ContentManifest.AssetEntry(bundleKey, deviceList, "unity3d");
		}
	}
}
