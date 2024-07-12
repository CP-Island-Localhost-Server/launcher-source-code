using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a Game Object is visible.")]
	[ActionTarget(typeof(GameObject), "gameObject", false)]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectIsVisible : ComponentAction<Renderer>
	{
		[Tooltip("The GameObject to test.")]
		[CheckForComponent(typeof(Renderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Event to send if the GameObject is visible.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if the GameObject is NOT visible.")]
		public FsmEvent falseEvent;

		[Tooltip("Store the result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsVisible();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsVisible();
		}

		private void DoIsVisible()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				bool isVisible = base.renderer.isVisible;
				storeResult.Value = isVisible;
				base.Fsm.Event(isVisible ? trueEvent : falseEvent);
			}
		}
	}
}
