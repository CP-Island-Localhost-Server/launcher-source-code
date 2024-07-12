using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the number of Game Objects in the scene with the specified Tag.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetTagCount : FsmStateAction
	{
		[UIHint(UIHint.Tag)]
		public FsmString tag;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt storeResult;

		public override void Reset()
		{
			tag = "Untagged";
			storeResult = null;
		}

		public override void OnEnter()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag.Value);
			if (storeResult != null)
			{
				storeResult.Value = ((array != null) ? array.Length : 0);
			}
			Finish();
		}
	}
}
