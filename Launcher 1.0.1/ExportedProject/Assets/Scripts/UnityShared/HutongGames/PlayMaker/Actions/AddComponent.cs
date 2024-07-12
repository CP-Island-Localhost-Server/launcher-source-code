using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a Component to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Component on exiting the state.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class AddComponent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to add the Component to.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.ScriptComponent)]
		[Title("Component Type")]
		[Tooltip("The type of Component to add to the Game Object.")]
		public FsmString component;

		[Tooltip("Store the component in an Object variable. E.g., to use with Set Property.")]
		[ObjectType(typeof(Component))]
		[UIHint(UIHint.Variable)]
		public FsmObject storeComponent;

		[Tooltip("Remove the Component when this State is exited.")]
		public FsmBool removeOnExit;

		private Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			component = null;
			storeComponent = null;
		}

		public override void OnEnter()
		{
			DoAddComponent();
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && addedComponent != null)
			{
				Object.Destroy(addedComponent);
			}
		}

		private void DoAddComponent()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				addedComponent = ownerDefaultTarget.AddComponent(ReflectionUtils.GetGlobalType(component.Value));
				storeComponent.Value = addedComponent;
				if (addedComponent == null)
				{
					LogError("Can't add component: " + component.Value);
				}
			}
		}
	}
}
