using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a named texture in a game object's material to a movie texture.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetMaterialMovieTexture : ComponentAction<Renderer>
	{
		[CheckForComponent(typeof(Renderer))]
		[Tooltip("The GameObject that the material is applied to.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		[UIHint(UIHint.NamedTexture)]
		[Tooltip("A named texture in the shader.")]
		public FsmString namedTexture;

		[ObjectType(typeof(MovieTexture))]
		[RequiredField]
		public FsmObject movieTexture;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			material = null;
			namedTexture = "_MainTex";
			movieTexture = null;
		}

		public override void OnEnter()
		{
			DoSetMaterialTexture();
			Finish();
		}

		private void DoSetMaterialTexture()
		{
			MovieTexture value = movieTexture.Value as MovieTexture;
			string text = namedTexture.Value;
			if (text == "")
			{
				text = "_MainTex";
			}
			if (material.Value != null)
			{
				material.Value.SetTexture(text, value);
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
					base.renderer.material.SetTexture(text, value);
				}
				else if (base.renderer.materials.Length > materialIndex.Value)
				{
					Material[] materials = base.renderer.materials;
					materials[materialIndex.Value].SetTexture(text, value);
					base.renderer.materials = materials;
				}
			}
		}
	}
}
