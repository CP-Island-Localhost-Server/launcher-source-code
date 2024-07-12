using System;
using System.Collections;
using System.Text;
using Disney.MobileNetwork;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace Disney.Kelowna.Common
{
	public class CPipeManifestService : ICPipeManifestService
	{
		private const string CPIPE_MAPPING_MANIFEST_KEY_CURRENT = "current";

		private const string CPIPE_MAPPING_MANIFEST_KEY_DEFAULT = "default";

		private const string CPIPE_MAPPING_MANIFEST_PATH_KEY = "n";

		private const string CDN_ROOT_MANIFEST_KEY = "cdnRoot";

		private const string PATHS_MANIFEST_KEY = "paths";

		private const string ASSET_VERSION_KEY = "v";

		private const string ASSET_CRC_KEY = "crc";

		private const string DISNEY_IO_SUFFIX = ".disney.io";

		private readonly string urlRoot;

		private readonly string cpipeMappingFilename;

		private readonly IGcsAccessTokenService gcsAccessTokenService;

		private bool isGettingManifest = false;

		private JsonData cachedCPipeManifestData;

		public CPipeManifestService(string urlRoot, string cpipeMappingFilename, IGcsAccessTokenService gcsAccessTokenService)
		{
			this.urlRoot = urlRoot;
			this.cpipeMappingFilename = cpipeMappingFilename;
			this.gcsAccessTokenService = gcsAccessTokenService;
		}

		public IEnumerator LookupAssetUrl(CPipeManifestResponse cpipeManifestResponse, string assetName)
		{
			cpipeManifestResponse.Clear();
			cpipeManifestResponse.AssetName = assetName;
			GcsAccessTokenResponse gcsAccessTokenResponse = new GcsAccessTokenResponse();
			while (isGettingManifest)
			{
				yield return null;
			}
			if (cachedCPipeManifestData == null)
			{
				try
				{
					isGettingManifest = true;
					cachedCPipeManifestData = null;
					yield return getCPipeManifestData(gcsAccessTokenResponse);
				}
				finally
				{
					isGettingManifest = false;
				}
			}
			JsonData pathsData = cachedCPipeManifestData["paths"];
			if (pathsData.Contains(assetName))
			{
				JsonData assetData = pathsData[assetName];
				yield return gcsAccessTokenService.GetAccessToken(gcsAccessTokenResponse);
				cpipeManifestResponse.FullAssetUrl = getFullAssetUrl(assetName, assetData, gcsAccessTokenResponse);
			}
		}

		private string getFullAssetUrl(string assetName, JsonData assetData, GcsAccessTokenResponse gcsAccessTokenResponse)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = (string)cachedCPipeManifestData["cdnRoot"];
			text = text.TrimEnd('/');
			stringBuilder.Append(text);
			if (!text.EndsWith(".disney.io"))
			{
				stringBuilder.Append(".disney.io");
			}
			stringBuilder.Append('/');
			stringBuilder.Append(assetData["v"]);
			stringBuilder.Append('/');
			stringBuilder.Append(assetName);
			appendAccessTokenParameter(stringBuilder, gcsAccessTokenResponse);
			return stringBuilder.ToString();
		}

		private IEnumerator getCPipeManifestData(GcsAccessTokenResponse gcsAccessTokenResponse)
		{
			yield return gcsAccessTokenService.GetAccessToken(gcsAccessTokenResponse);
			StringBuilder manifestUrl = new StringBuilder();
			setUrlRoot(manifestUrl);
			manifestUrl.Append(cpipeMappingFilename);
			appendAccessTokenParameter(manifestUrl, gcsAccessTokenResponse);
			appendCacheBusterParameter(manifestUrl);
			UnityWebRequest wwwManifestInfo = UnityWebRequest.Get(manifestUrl.ToString());
			bool useLoadingController = Service.IsSet<LoadingController>();
			if (useLoadingController)
			{
				Service.Get<LoadingController>().RegisterDownload(wwwManifestInfo);
			}
			yield return wwwManifestInfo.Send();
			if (useLoadingController)
			{
				Service.Get<LoadingController>().UnRegisterDownload(wwwManifestInfo);
			}
			if (wwwManifestInfo.isError)
			{
				throw new Exception(wwwManifestInfo.error);
			}
			JsonData manifestInfoData = JsonMapper.ToObject(wwwManifestInfo.downloadHandler.text);
			if (manifestInfoData == null || !manifestInfoData.Contains("default") || !manifestInfoData["default"].Contains("n"))
			{
				throw new Exception(string.Format("Invalid JSON Manifest Info: {0}", wwwManifestInfo.downloadHandler.text));
			}
			string clientVersion = ClientInfo.GetClientVersionStr();
			string manifestPath = (manifestInfoData.Keys.Contains(clientVersion) ? ((string)manifestInfoData[clientVersion]["n"]) : ((!manifestInfoData.Keys.Contains("current")) ? ((string)manifestInfoData["default"]["n"]) : ((string)manifestInfoData["current"]["n"])));
			setUrlRoot(manifestUrl);
			manifestUrl.Append(manifestPath);
			appendAccessTokenParameter(manifestUrl, gcsAccessTokenResponse);
			UnityWebRequest wwwManifest = UnityWebRequest.Get(manifestUrl.ToString());
			if (useLoadingController)
			{
				Service.Get<LoadingController>().RegisterDownload(wwwManifest);
			}
			yield return wwwManifest.Send();
			if (useLoadingController)
			{
				Service.Get<LoadingController>().UnRegisterDownload(wwwManifest);
			}
			if (wwwManifest.isError)
			{
				throw new Exception(wwwManifest.error);
			}
			cachedCPipeManifestData = JsonMapper.ToObject(wwwManifest.downloadHandler.text);
			if (cachedCPipeManifestData == null)
			{
				throw new Exception(string.Format("Invalid JSON Manifest File: {0}", wwwManifest.downloadHandler.text));
			}
		}

		private void setUrlRoot(StringBuilder urlStringBuilder)
		{
			urlStringBuilder.Remove(0, urlStringBuilder.Length);
			if (!string.IsNullOrEmpty(urlRoot))
			{
				urlStringBuilder.Append(urlRoot);
				if (!urlRoot.EndsWith("/"))
				{
					urlStringBuilder.Append('/');
				}
			}
		}

		private static void appendAccessTokenParameter(StringBuilder url, GcsAccessTokenResponse gcsAccessTokenResponse)
		{
			appendGenericParameter(url, "access_token", gcsAccessTokenResponse.AccessToken);
		}

		private static void appendCacheBusterParameter(StringBuilder url)
		{
			appendGenericParameter(url, "cacheBust", DateTimeOffset.UtcNow.ToString());
		}

		private static void appendGenericParameter(StringBuilder url, string paramName, string paramValue)
		{
			if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(paramValue))
			{
				if (url.ToString().Contains("?"))
				{
					url.Append("&");
				}
				else
				{
					url.Append("?");
				}
				url.Append(WWW.EscapeURL(paramName));
				url.Append("=");
				url.Append(WWW.EscapeURL(paramValue));
			}
		}
	}
}
