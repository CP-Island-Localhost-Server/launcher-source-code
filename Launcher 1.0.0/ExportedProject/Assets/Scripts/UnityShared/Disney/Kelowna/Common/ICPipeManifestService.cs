using System.Collections;

namespace Disney.Kelowna.Common
{
	public interface ICPipeManifestService
	{
		IEnumerator LookupAssetUrl(CPipeManifestResponse cpipeManifestResponse, string assetName);
	}
}
