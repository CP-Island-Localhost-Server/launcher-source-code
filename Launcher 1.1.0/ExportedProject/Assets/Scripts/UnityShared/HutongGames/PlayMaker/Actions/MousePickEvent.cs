using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends Events based on mouse interactions with a Game Object: MouseOver, MouseDown, MouseUp, MouseOff. Use Ray Distance to set how close the camera must be to pick the object.\n\nNOTE: Picking uses the Main Camera.")]
	[ActionTarget(typeof(GameObject), "GameObject", false)]
	[ActionCategory(ActionCategory.Input)]
	public class MousePickEvent : FsmStateAction
	{
		[CheckForComponent(typeof(Collider))]
		public FsmOwnerDefault GameObject;

		[Tooltip("Length of the ray to cast from the camera.")]
		public FsmFloat rayDistance = 100f;

		[Tooltip("Event to send when the mouse is over the GameObject.")]
		public FsmEvent mouseOver;

		[Tooltip("Event to send when the mouse is pressed while over the GameObject.")]
		public FsmEvent mouseDown;

		[Tooltip("Event to send when the mouse is released while over the GameObject.")]
		public FsmEvent mouseUp;

		[Tooltip("Event to send when the mouse moves off the GameObject.")]
		public FsmEvent mouseOff;

		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			GameObject = null;
			rayDistance = 100f;
			mouseOver = null;
			mouseDown = null;
			mouseUp = null;
			mouseOff = null;
			layerMask = new FsmInt[0];
			invertMask = false;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoMousePickEvent();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMousePickEvent();
		}

		private void DoMousePickEvent()
		{
			bool flag = DoRaycast();
			base.Fsm.RaycastHitInfo = ActionHelpers.mousePickInfo;
			if (flag)
			{
				if (mouseDown != null && Input.GetMouseButtonDown(0))
				{
					base.Fsm.Event(mouseDown);
				}
				if (mouseOver != null)
				{
					base.Fsm.Event(mouseOver);
				}
				if (mouseUp != null && Input.GetMouseButtonUp(0))
				{
					base.Fsm.Event(mouseUp);
				}
			}
			else if (mouseOff != null)
			{
				base.Fsm.Event(mouseOff);
			}
		}

		private bool DoRaycast()
		{
			GameObject gameObject = ((GameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : GameObject.GameObject.Value);
			return ActionHelpers.IsMouseOver(gameObject, rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
		}

		public override string ErrorCheck()
		{
			string text = "";
			text += ActionHelpers.CheckRayDistance(rayDistance.Value);
			return text + ActionHelpers.CheckPhysicsSetup(GameObject);
		}
	}
}
