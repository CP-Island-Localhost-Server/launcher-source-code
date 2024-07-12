using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the name of a Game Object and stores it in a String Variable.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetName : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeName;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = new FsmGameObject
			{
				UseVariable = true
			};
			storeName = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetGameObjectName();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetGameObjectName();
		}

		private void DoGetGameObjectName()
		{
			GameObject value = gameObject.Value;
			storeName.Value = ((value != null) ? value.name : "");
		}
	}
}
