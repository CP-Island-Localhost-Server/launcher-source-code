using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Mass of a Game Object's Rigid Body 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class SetMass2d : ComponentAction<Rigidbody2D>
	{
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The Mass")]
		[RequiredField]
		[HasFloatSlider(0.1f, 10f)]
		public FsmFloat mass;

		public override void Reset()
		{
			gameObject = null;
			mass = 1f;
		}

		public override void OnEnter()
		{
			DoSetMass();
			Finish();
		}

		private void DoSetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.rigidbody2d.mass = mass.Value;
			}
		}
	}
}
