using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	public class BundlePrecacheManager
	{
		public delegate void PrecacheComplete();

		private ContentManifest manifest;

		private ContentPreCachingConfig config;

		private PrecacheComplete completeCallback = null;

		private int numCurrentDownloads = 0;

		private bool caching = false;

		private bool background = false;

		private float completeRatio = 0f;

		public bool IsReady
		{
			get
			{
				return config != null;
			}
		}

		public float CompleteRatio
		{
			get
			{
				return completeRatio;
			}
		}

		public ContentPreCachingConfig Config
		{
			get
			{
				return config;
			}
		}

		public BundlePrecacheManager(ContentManifest contentManifest)
		{
			SetManifest(contentManifest);
			CoroutineRunner.StartPersistent(loadConfig(), this, "BundlePrecacheManager.loadConfig()");
		}

		public void SetManifest(ContentManifest contentManifest)
		{
			manifest = contentManifest;
		}

		private IEnumerator loadConfig()
		{
			AssetRequest<ContentPreCachingConfig> request = Content.LoadAsync<ContentPreCachingConfig>("Configuration/ContentPreCachingConfig");
			yield return request;
			config = request.Asset;
		}

		public void StartCaching(PrecacheComplete callback, bool cacheInBackground = false)
		{
			caching = true;
			background = cacheInBackground;
			completeCallback = callback;
			CoroutineRunner.StartPersistent(cacheBundles(), this, "BundlePrecacheManager.cacheBundles()");
		}

		public void MoveToBackground()
		{
			background = true;
		}

		public void StopCaching()
		{
			caching = false;
		}

		public IEnumerator cacheBundles()
		{
			List<ContentManifest.BundleEntry> sortedBundles = new List<ContentManifest.BundleEntry>(manifest.BundleEntryMap.Values);
			sortedBundles.Sort(compareBundlesByPriority);
			for (int i = 0; i < sortedBundles.Count; i++)
			{
				completeRatio = (float)(i - numCurrentDownloads) / (float)sortedBundles.Count;
				uint maxConcurrentDownloads = (background ? config.MaxConcurrentBackgroundDownloads : config.MaxConcurrentForegroundDownloads);
				while (numCurrentDownloads >= maxConcurrentDownloads)
				{
					yield return null;
					maxConcurrentDownloads = (background ? config.MaxConcurrentBackgroundDownloads : config.MaxConcurrentForegroundDownloads);
				}
				if (!caching)
				{
					break;
				}
				ContentManifest.BundleEntry bundle = sortedBundles[i];
				if (bundle.Priority > 0)
				{
					CoroutineRunner.StartPersistent(downloadBundle(bundle), this, "BundlePrecacheManager.downloadBundle()");
					continue;
				}
				break;
			}
			if (sortedBundles.Count > 0)
			{
				while (numCurrentDownloads > 0)
				{
					yield return null;
					completeRatio = (float)(sortedBundles.Count - numCurrentDownloads) / (float)sortedBundles.Count;
				}
			}
			if (caching && completeCallback != null)
			{
				completeCallback();
			}
		}

		private IEnumerator downloadBundle(ContentManifest.BundleEntry bundleEntry)
		{
			ContentManifest.AssetEntry entry = new ContentManifest.AssetEntry(bundleEntry.Key, "www-bundle", "unity3d");
			numCurrentDownloads++;
			yield return Content.DeviceManager.LoadAsync<AssetBundle>("www-bundle", ref entry);
			numCurrentDownloads--;
		}

		private static int compareBundlesByPriority(ContentManifest.BundleEntry bundle1, ContentManifest.BundleEntry bundle2)
		{
			return bundle2.Priority.CompareTo(bundle1.Priority);
		}
	}
}
