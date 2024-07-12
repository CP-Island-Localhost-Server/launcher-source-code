using System.Collections;
using UnityEngine;

namespace Disney.Kelowna.Common.Tests
{
	public class Content_LoadFromIndex_Test : BaseContentIntegrationTest
	{
		protected override IEnumerator runTest()
		{
			TextAsset textAsset = Content.LoadImmediate<TextAsset>("small_text");
			TextAsset textAsset2 = Content.LoadImmediate<TextAsset>("small_text");
			IntegrationTest.Assert(textAsset == textAsset2);
			yield break;
		}
	}
}
