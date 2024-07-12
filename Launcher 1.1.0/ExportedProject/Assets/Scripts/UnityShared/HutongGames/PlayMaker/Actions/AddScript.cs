using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a Script to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Script on exiting the state.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class AddScript : FsmStateAction
	{
		[Tooltip("The GameObject to add the script to.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.ScriptComponent)]
		[Tooltip("The Script to add to the GameObject.")]
		[RequiredField]
		public FsmString script;

		[Tooltip("Remove the script from the GameObject when this State is exited.")]
		public FsmBool removeOnExit;

		private Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			script = null;
		}

		public override void OnEnter()
		{
			DoAddComponent((gameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : gameObject.GameObject.Value);
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && addedComponent != null)
			{
				Object.Destroy(addedComponent);
			}
		}

		private void DoAddComponent(GameObject go)
		{
			addedComponent = go.AddComponent(ReflectionUtils.GetGlobalType(script.Value));
			if (addedComponent == null)
			{
				LogError("Can't add script: " + script.Value);
			}
		}
	}
}
