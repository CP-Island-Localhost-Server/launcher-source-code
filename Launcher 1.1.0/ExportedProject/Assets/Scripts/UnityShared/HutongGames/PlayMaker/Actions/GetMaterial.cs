using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a material at index on a gameObject and store it in a variable")]
	[ActionCategory(ActionCategory.Material)]
	public class GetMaterial : ComponentAction<Renderer>
	{
		[Tooltip("The GameObject the Material is applied to.")]
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The index of the Material in the Materials array.")]
		public FsmInt materialIndex;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the material in a variable.")]
		[RequiredField]
		public FsmMaterial material;

		[Tooltip("Get the shared material of this object. NOTE: Modifying the shared material will change the appearance of all objects using this material, and change material settings that are stored in the project too.")]
		public bool getSharedMaterial;

		public override void Reset()
		{
			gameObject = null;
			material = null;
			materialIndex = 0;
			getSharedMaterial = false;
		}

		public override void OnEnter()
		{
			DoGetMaterial();
			Finish();
		}

		private void DoGetMaterial()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				if (materialIndex.Value == 0 && !getSharedMaterial)
				{
					material.Value = base.renderer.material;
				}
				else if (materialIndex.Value == 0 && getSharedMaterial)
				{
					material.Value = base.renderer.sharedMaterial;
				}
				else if (base.renderer.materials.Length > materialIndex.Value && !getSharedMaterial)
				{
					Material[] materials = base.renderer.materials;
					material.Value = materials[materialIndex.Value];
					base.renderer.materials = materials;
				}
				else if (base.renderer.materials.Length > materialIndex.Value && getSharedMaterial)
				{
					Material[] materials = base.renderer.sharedMaterials;
					material.Value = materials[materialIndex.Value];
					base.renderer.sharedMaterials = materials;
				}
			}
		}
	}
}
