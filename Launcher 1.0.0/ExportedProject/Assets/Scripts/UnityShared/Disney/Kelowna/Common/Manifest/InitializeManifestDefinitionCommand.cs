using System;
using System.Collections;
using System.IO;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.Networking;

namespace Disney.Kelowna.Common.Manifest
{
	public class InitializeManifestDefinitionCommand
	{
		public delegate void ManifestInitializationComplete(ContentManifest manifest, bool requiresAppUgrade = false, bool appUgradeAvailable = false);

		private ManifestInitializationComplete callback;

		private IManifestService manifestService;

		public ContentManifestDirectory manifestDirectory;

		public ContentManifest embeddedManifest;

		public ContentManifest contentManifest;

		private ContentManifest cachedCdnManifest;

		private ContentManifest downloadedManifest;

		private bool appUpgradeAvailable;

		private readonly CoroutineGroup activeCoroutines = new CoroutineGroup();

		private ICoroutine timeoutCheck;

		public InitializeManifestDefinitionCommand(IManifestService service, ManifestInitializationComplete callback)
		{
			manifestService = service;
			this.callback = callback;
		}

		private void onInitializeComplete(ContentManifest manifest, bool requiresAppUgrade = false)
		{
			if (timeoutCheck != null && !timeoutCheck.Completed && !timeoutCheck.Cancelled)
			{
				timeoutCheck.Cancel();
			}
			manifest.UpdateEntryMaps();
			callback(manifest, requiresAppUgrade, appUpgradeAvailable);
		}

		public void Execute()
		{
			try
			{
				timeoutCheck = CoroutineRunner.Start(timeout_loadManifestSequence(), this, "timeoutLoadPatchManifest");
				activeCoroutines.Add(timeoutCheck);
				activeCoroutines.StartAndAdd(loadContentManifest(), this, "loadContentManifest");
			}
			catch (Exception ex)
			{
				Log.LogException(this, ex);
				handleFailure();
			}
		}

		private IEnumerator loadContentManifest()
		{
			yield return manifestDirectory_download();
			if (manifestDirectory == null)
			{
				Log.LogError(this, "The ContentManifestDirectory downloaded is null! The request to download the directory either was not made or failed to download and deserialize. Reverting to the embedded ContentManifest as no ContentManifestDirectoryEntry is available to reference.");
				handleFailure();
				yield break;
			}
			embeddedManifest = manifestService.LoadEmbeddedManifest();
			cachedCdnManifest = manifestService.LoadCachedCdnManifest();
			if (cachedCdnManifest == null)
			{
			}
			DateTimeOffset contentDate = manifestService.GetContentDate();
			Version clientVersion = ClientInfo.ParseClientVersion(manifestService.GetClientVersionStr());
			string platform = manifestService.GetClientPlatform();
			string environment = manifestService.GetServerEnvironment().ToString().ToLower();
			ContentManifestDirectoryEntry directoryEntry = manifestDirectory.FindEntry(contentDate, clientVersion, platform, environment);
			appUpgradeAvailable = manifestDirectory.DoesNewerVersionExist(contentDate, clientVersion, platform, environment);
			if (directoryEntry == null)
			{
				Log.LogErrorFormatted(this, "No content available for clientVersion={0}, platform={1}, environment={2}. \nTriggering force upgrade.", clientVersion.ToString(), platform, environment);
				handleFailure(true);
				yield break;
			}
			if (directoryEntry.IsEmbeddedContent())
			{
				contentManifest = embeddedManifest;
				checkContentVersionAndManifestHashInManifest(contentManifest, directoryEntry);
			}
			else
			{
				string contentManifestHash = Path.GetFileNameWithoutExtension(directoryEntry.url);
				if (cachedCdnManifest != null && cachedCdnManifest.ContentVersion.Equals(directoryEntry.contentVersion) && cachedCdnManifest.ContentManifestHash.Equals(contentManifestHash))
				{
					contentManifest = ContentManifestUtility.Merge(embeddedManifest, cachedCdnManifest);
				}
				else
				{
					yield return contentManifest_download(directoryEntry);
					contentManifest = ContentManifestUtility.Merge(embeddedManifest, downloadedManifest);
				}
			}
			onInitializeComplete(contentManifest);
		}

		private void checkContentVersionAndManifestHashInManifest(ContentManifest manifest, ContentManifestDirectoryEntry directoryEntry)
		{
			if (manifest == null || directoryEntry == null)
			{
				Log.LogErrorFormatted(this, "Can't check on manifest since one of the parameters is null manifest={0}, directoryEntry={1}\t", manifest, directoryEntry);
				return;
			}
			if (string.IsNullOrEmpty(manifest.ContentManifestHash) && !string.IsNullOrEmpty(directoryEntry.url))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(directoryEntry.url);
				manifest.ContentManifestHash = fileNameWithoutExtension;
			}
			if (string.IsNullOrEmpty(manifest.ContentVersion))
			{
				manifest.ContentVersion = directoryEntry.contentVersion;
			}
			else if (manifest.ContentVersion != directoryEntry.contentVersion)
			{
				manifest.ContentVersion = directoryEntry.contentVersion;
			}
		}

		private IEnumerator manifestDirectory_download()
		{
			ICPipeManifestService cpipeManifestService = Service.Get<ICPipeManifestService>();
			CPipeManifestResponse cpipeManifestResponse = new CPipeManifestResponse();
			yield return cpipeManifestService.LookupAssetUrl(cpipeManifestResponse, "ClientManifestDirectory.json");
			UnityWebRequest www = UnityWebRequest.Get(cpipeManifestResponse.FullAssetUrl);
			Service.Get<LoadingController>().RegisterDownload(www);
			yield return www.Send();
			Service.Get<LoadingController>().UnRegisterDownload(www);
			try
			{
				if (www.isError || string.IsNullOrEmpty(www.downloadHandler.text))
				{
					throw new UnityException("Failed to download manifest directory from CDN. Reverting to the embedded directory.");
				}
				manifestDirectory = Service.Get<JsonService>().Deserialize<ContentManifestDirectory>(www.downloadHandler.text);
			}
			catch (Exception)
			{
				manifestDirectory = manifestDirectory_loadEmbedded();
				onInitializeComplete(manifestService.LoadEmbeddedManifest());
			}
		}

		private ContentManifestDirectory manifestDirectory_loadEmbedded()
		{
			TextAsset textAsset = Resources.Load<TextAsset>("Configuration/embedded_manifest_directory.json");
			return Service.Get<JsonService>().Deserialize<ContentManifestDirectory>(textAsset.text);
		}

		private IEnumerator contentManifest_download(ContentManifestDirectoryEntry directoryEntry)
		{
			if (directoryEntry == null)
			{
				handleFailure();
				yield break;
			}
			if (string.IsNullOrEmpty(directoryEntry.url))
			{
				handleFailure();
				yield break;
			}
			string contentManifestUrl = directoryEntry.url;
			ICPipeManifestService cpipeManifestService = Service.Get<ICPipeManifestService>();
			CPipeManifestResponse cpipeManifestResponse = new CPipeManifestResponse();
			yield return cpipeManifestService.LookupAssetUrl(cpipeManifestResponse, contentManifestUrl);
			UnityWebRequest www = UnityWebRequest.GetAssetBundle(cpipeManifestResponse.FullAssetUrl, 1u, 0u);
			Service.Get<LoadingController>().RegisterDownload(www);
			yield return www.Send();
			Service.Get<LoadingController>().UnRegisterDownload(www);
			AssetBundle contentBundle = ((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle;
			if (www.isError || contentBundle == null)
			{
				Log.LogErrorFormatted(this, "Failed to download content bundle from CDN: {0}", www.error);
				handleFailure();
				yield break;
			}
			string[] assetNames = contentBundle.GetAllAssetNames();
			for (int i = 0; i < assetNames.Length; i++)
			{
				if (Path.GetFileNameWithoutExtension(assetNames[i]).ToLower() == "contentmanifest")
				{
					TextAsset textAsset = contentBundle.LoadAsset<TextAsset>(assetNames[i]);
					downloadedManifest = new ContentManifest(textAsset);
					checkContentVersionAndManifestHashInManifest(downloadedManifest, directoryEntry);
					contentManifest_cacheDownloadedToDisk(downloadedManifest);
					break;
				}
			}
			contentBundle.Unload(false);
			if (downloadedManifest == null)
			{
				Log.LogError(this, "Content manifest not found in downloaded bundle! Reverting to the embedded manifest");
				handleFailure();
			}
		}

		private void contentManifest_cacheDownloadedToDisk(ContentManifest manifestToBeCached)
		{
			string path = Path.Combine(Application.persistentDataPath, "ContentManifest.txt");
			manifestToBeCached.WriteToFile(path);
		}

		private ContentManifest contentManifest_loadFromCache()
		{
			string path = Path.Combine(Application.persistentDataPath, "ContentManifest.txt");
			if (File.Exists(path))
			{
				ContentManifest contentManifest = new ContentManifest();
				contentManifest.ReadFromFile(path);
				return contentManifest;
			}
			return null;
		}

		private ContentManifest contentManifest_loadEmbedded()
		{
			return manifestService.LoadEmbeddedManifest();
		}

		public static void contentManifest_clearCachedManifest()
		{
			string path = Path.Combine(Application.persistentDataPath, "ContentManifest.txt");
			File.Delete(path);
		}

		private IEnumerator timeout_loadManifestSequence()
		{
			yield return new WaitForSeconds(10f);
			onInitializeComplete(contentManifest_loadEmbedded());
		}

		private void handleFailure(bool requiresAppUgrade = false)
		{
			activeCoroutines.StopAll();
			onInitializeComplete(contentManifest_loadEmbedded(), requiresAppUgrade);
		}
	}
}
