using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Game Object's material randomly from an array of Materials.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetRandomMaterial : ComponentAction<Renderer>
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		public FsmInt materialIndex;

		public FsmMaterial[] materials;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			materials = new FsmMaterial[3];
		}

		public override void OnEnter()
		{
			DoSetRandomMaterial();
			Finish();
		}

		private void DoSetRandomMaterial()
		{
			if (materials == null || materials.Length == 0)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				if (base.renderer.material == null)
				{
					LogError("Missing Material!");
				}
				else if (materialIndex.Value == 0)
				{
					base.renderer.material = materials[Random.Range(0, materials.Length)].Value;
				}
				else if (base.renderer.materials.Length > materialIndex.Value)
				{
					Material[] array = base.renderer.materials;
					array[materialIndex.Value] = materials[Random.Range(0, materials.Length)].Value;
					base.renderer.materials = array;
				}
			}
		}
	}
}
