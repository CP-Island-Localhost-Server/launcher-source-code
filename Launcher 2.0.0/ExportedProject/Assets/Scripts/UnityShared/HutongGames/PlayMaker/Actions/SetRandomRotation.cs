using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets Random Rotation for a Game Object. Uncheck an axis to keep its current value.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SetRandomRotation : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmBool x;

		[RequiredField]
		public FsmBool y;

		[RequiredField]
		public FsmBool z;

		public override void Reset()
		{
			gameObject = null;
			x = true;
			y = true;
			z = true;
		}

		public override void OnEnter()
		{
			DoRandomRotation();
			Finish();
		}

		private void DoRandomRotation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				Vector3 localEulerAngles = ownerDefaultTarget.transform.localEulerAngles;
				float num = localEulerAngles.x;
				float num2 = localEulerAngles.y;
				float num3 = localEulerAngles.z;
				if (x.Value)
				{
					num = Random.Range(0, 360);
				}
				if (y.Value)
				{
					num2 = Random.Range(0, 360);
				}
				if (z.Value)
				{
					num3 = Random.Range(0, 360);
				}
				ownerDefaultTarget.transform.localEulerAngles = new Vector3(num, num2, num3);
			}
		}
	}
}
