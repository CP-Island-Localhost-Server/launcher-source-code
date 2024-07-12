using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Enables/Disables a Behaviour on a GameObject. Optionally reset the Behaviour on exit - useful if you want the Behaviour to be active only while this state is active.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class EnableBehaviour : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the Behaviour.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Behaviour)]
		[Tooltip("The name of the Behaviour to enable/disable.")]
		public FsmString behaviour;

		[Tooltip("Optionally drag a component directly into this field (behavior name will be ignored).")]
		public Component component;

		[Tooltip("Set to True to enable, False to disable.")]
		[RequiredField]
		public FsmBool enable;

		public FsmBool resetOnExit;

		private Behaviour componentTarget;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			component = null;
			enable = true;
			resetOnExit = true;
		}

		public override void OnEnter()
		{
			DoEnableBehaviour(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void DoEnableBehaviour(GameObject go)
		{
			if (!(go == null))
			{
				if (component != null)
				{
					componentTarget = component as Behaviour;
				}
				else
				{
					componentTarget = go.GetComponent(ReflectionUtils.GetGlobalType(behaviour.Value)) as Behaviour;
				}
				if (componentTarget == null)
				{
					LogWarning(" " + go.name + " missing behaviour: " + behaviour.Value);
				}
				else
				{
					componentTarget.enabled = enable.Value;
				}
			}
		}

		public override void OnExit()
		{
			if (!(componentTarget == null) && resetOnExit.Value)
			{
				componentTarget.enabled = !enable.Value;
			}
		}

		public override string ErrorCheck()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null || component != null || this.behaviour.IsNone || string.IsNullOrEmpty(this.behaviour.Value))
			{
				return null;
			}
			Behaviour behaviour = ownerDefaultTarget.GetComponent(ReflectionUtils.GetGlobalType(this.behaviour.Value)) as Behaviour;
			return (behaviour != null) ? null : "Behaviour missing";
		}
	}
}
