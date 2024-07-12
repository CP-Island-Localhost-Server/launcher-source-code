using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Color of the GUITexture attached to a Game Object.")]
	public class SetGUITextureColor : ComponentAction<GUITexture>
	{
		[CheckForComponent(typeof(GUITexture))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmColor color;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			color = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetGUITextureColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetGUITextureColor();
		}

		private void DoSetGUITextureColor()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.guiTexture.color = color.Value;
			}
		}
	}
}
