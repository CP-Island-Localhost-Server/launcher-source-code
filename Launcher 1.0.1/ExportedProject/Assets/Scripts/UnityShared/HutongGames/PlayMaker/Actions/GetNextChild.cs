using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Each time this action is called it gets the next child of a GameObject. This lets you quickly loop through all the children of an object to perform actions on them. NOTE: To find a specific child use Find Child.")]
	public class GetNextChild : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The parent GameObject. Note, if GameObject changes, this action will reset and start again at the first child.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the next child in a GameObject variable.")]
		public FsmGameObject storeNextChild;

		[Tooltip("Event to send to get the next child.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send when there are no more children.")]
		public FsmEvent finishedEvent;

		private GameObject go;

		private int nextChildIndex;

		public override void Reset()
		{
			gameObject = null;
			storeNextChild = null;
			loopEvent = null;
			finishedEvent = null;
		}

		public override void OnEnter()
		{
			DoGetNextChild(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void DoGetNextChild(GameObject parent)
		{
			if (parent == null)
			{
				return;
			}
			if (go != parent)
			{
				go = parent;
				nextChildIndex = 0;
			}
			if (nextChildIndex >= go.transform.childCount)
			{
				nextChildIndex = 0;
				base.Fsm.Event(finishedEvent);
				return;
			}
			storeNextChild.Value = parent.transform.GetChild(nextChildIndex).gameObject;
			if (nextChildIndex >= go.transform.childCount)
			{
				nextChildIndex = 0;
				base.Fsm.Event(finishedEvent);
				return;
			}
			nextChildIndex++;
			if (loopEvent != null)
			{
				base.Fsm.Event(loopEvent);
			}
		}
	}
}
