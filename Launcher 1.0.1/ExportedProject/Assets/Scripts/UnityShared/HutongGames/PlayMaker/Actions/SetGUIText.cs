using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Text used by the GUIText Component attached to a Game Object.")]
	[ActionCategory(ActionCategory.GUIElement)]
	public class SetGUIText : ComponentAction<GUIText>
	{
		[CheckForComponent(typeof(GUIText))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.TextArea)]
		public FsmString text;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			text = "";
		}

		public override void OnEnter()
		{
			DoSetGUIText();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetGUIText();
		}

		private void DoSetGUIText()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.guiText.text = text.Value;
			}
		}
	}
}
