using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Makes the Game Object not be destroyed automatically when loading a new scene.")]
	[ActionCategory(ActionCategory.Level)]
	public class DontDestroyOnLoad : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject to mark as DontDestroyOnLoad.")]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			Object.DontDestroyOnLoad(base.Owner.transform.root.gameObject);
			Finish();
		}
	}
}
