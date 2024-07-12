using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the material on a game object.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetMaterial : ComponentAction<Renderer>
	{
		[CheckForComponent(typeof(Renderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmInt materialIndex;

		[RequiredField]
		public FsmMaterial material;

		public override void Reset()
		{
			gameObject = null;
			material = null;
			materialIndex = 0;
		}

		public override void OnEnter()
		{
			DoSetMaterial();
			Finish();
		}

		private void DoSetMaterial()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				if (materialIndex.Value == 0)
				{
					base.renderer.material = material.Value;
				}
				else if (base.renderer.materials.Length > materialIndex.Value)
				{
					Material[] materials = base.renderer.materials;
					materials[materialIndex.Value] = material.Value;
					base.renderer.materials = materials;
				}
			}
		}
	}
}
