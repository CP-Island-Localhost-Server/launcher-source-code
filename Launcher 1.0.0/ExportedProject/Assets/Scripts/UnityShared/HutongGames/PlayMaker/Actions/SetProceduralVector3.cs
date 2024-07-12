using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Substance")]
	[Tooltip("Set a named Vector3 property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	public class SetProceduralVector3 : FsmStateAction
	{
		[Tooltip("The Substance Material.")]
		[RequiredField]
		public FsmMaterial substanceMaterial;

		[Tooltip("The named vector property in the material.")]
		[RequiredField]
		public FsmString vector3Property;

		[Tooltip("The value to set the property to.\nNOTE: Use Set Procedural Vector3 for Vector3 values.")]
		[RequiredField]
		public FsmVector3 vector3Value;

		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;

		public override void Reset()
		{
			substanceMaterial = null;
			vector3Property = null;
			vector3Value = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetProceduralVector();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetProceduralVector();
		}

		private void DoSetProceduralVector()
		{
			ProceduralMaterial proceduralMaterial = substanceMaterial.Value as ProceduralMaterial;
			if (proceduralMaterial == null)
			{
				LogError("The Material is not a Substance Material!");
			}
			else
			{
				proceduralMaterial.SetProceduralVector(vector3Property.Value, vector3Value.Value);
			}
		}
	}
}
