using System;
using System.Collections;
using Disney.LaunchPadFramework;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	public class GcsReadOnlyClient
	{
		public class ReadResponse
		{
			public string Data;
		}

		protected readonly string bucket;

		protected readonly IGcsAccessTokenService gcsAccessTokenService;

		public GcsReadOnlyClient(string bucket, IGcsAccessTokenService gcsAccessTokenService)
		{
			this.bucket = bucket;
			this.gcsAccessTokenService = gcsAccessTokenService;
			gcsAccessTokenService.AccessType = GcsAccessType.READ_ONLY;
		}

		private string getReadAssetUrl(string assetName, string accessToken)
		{
			return string.Format("https://www.googleapis.com/storage/v1/b/{0}/o/{1}?alt=media&access_token={2}", bucket, Uri.EscapeDataString(assetName), accessToken);
		}

		public IEnumerator ReadJson(string assetName, ReadResponse response)
		{
			GcsAccessTokenResponse gcsAccessTokenResponse = new GcsAccessTokenResponse();
			yield return gcsAccessTokenService.GetAccessToken(gcsAccessTokenResponse);
			yield return followRedirects(getReadAssetUrl(assetName, gcsAccessTokenResponse.AccessToken), response);
		}

		private IEnumerator followRedirects(string url, ReadResponse response)
		{
			WWW request = new WWW(url);
			yield return request;
			if (!string.IsNullOrEmpty(request.error))
			{
				Log.LogErrorFormatted(this, "GCS request to {0} failed with error: {1}", request.url, request.error);
			}
			else if (request.responseHeaders["STATUS"].EndsWith("200 OK"))
			{
				response.Data = request.text;
			}
			else if (request.responseHeaders.ContainsKey("LOCATION"))
			{
				yield return followRedirects(request.responseHeaders["LOCATION"], response);
			}
			else
			{
				Log.LogErrorFormatted(this, "GCS request to {0} failed with status: {1} and body: {2}", request.url, request.responseHeaders["STATUS"], request.text);
			}
		}
	}
}
