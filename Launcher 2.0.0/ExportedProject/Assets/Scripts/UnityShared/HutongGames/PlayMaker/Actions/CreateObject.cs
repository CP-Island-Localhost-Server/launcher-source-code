using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Creates a Game Object, usually using a Prefab.")]
	[ActionTarget(typeof(GameObject), "gameObject", true)]
	public class CreateObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject to create. Usually a Prefab.")]
		public FsmGameObject gameObject;

		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject spawnPoint;

		[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;

		[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the created object.")]
		public FsmGameObject storeObject;

		public override void Reset()
		{
			gameObject = null;
			spawnPoint = null;
			position = new FsmVector3
			{
				UseVariable = true
			};
			rotation = new FsmVector3
			{
				UseVariable = true
			};
			storeObject = null;
		}

		public override void OnEnter()
		{
			GameObject value = gameObject.Value;
			if (value != null)
			{
				Vector3 vector = Vector3.zero;
				Vector3 euler = Vector3.zero;
				if (spawnPoint.Value != null)
				{
					vector = spawnPoint.Value.transform.position;
					if (!position.IsNone)
					{
						vector += position.Value;
					}
					euler = ((!rotation.IsNone) ? rotation.Value : spawnPoint.Value.transform.eulerAngles);
				}
				else
				{
					if (!position.IsNone)
					{
						vector = position.Value;
					}
					if (!rotation.IsNone)
					{
						euler = rotation.Value;
					}
				}
				GameObject value2 = Object.Instantiate(value, vector, Quaternion.Euler(euler));
				storeObject.Value = value2;
			}
			Finish();
		}
	}
}
