using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Checks if an Object has a Component. Optionally remove the Component on exiting the state.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class HasComponent : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.ScriptComponent)]
		[RequiredField]
		public FsmString component;

		public FsmBool removeOnExit;

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool store;

		public bool everyFrame;

		private Component aComponent;

		public override void Reset()
		{
			aComponent = null;
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			component = null;
			store = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoHasComponent((gameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : gameObject.GameObject.Value);
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoHasComponent((gameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : gameObject.GameObject.Value);
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && aComponent != null)
			{
				Object.Destroy(aComponent);
			}
		}

		private void DoHasComponent(GameObject go)
		{
			if (go == null)
			{
				if (!store.IsNone)
				{
					store.Value = false;
				}
				base.Fsm.Event(falseEvent);
				return;
			}
			aComponent = go.GetComponent(ReflectionUtils.GetGlobalType(component.Value));
			if (!store.IsNone)
			{
				store.Value = aComponent != null;
			}
			base.Fsm.Event((aComponent != null) ? trueEvent : falseEvent);
		}
	}
}