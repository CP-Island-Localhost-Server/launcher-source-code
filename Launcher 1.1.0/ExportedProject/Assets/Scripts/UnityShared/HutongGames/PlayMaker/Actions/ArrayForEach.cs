using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Iterate through the items in an Array and run an FSM on each item. NOTE: The FSM has to Finish before being run on the next item.")]
	public class ArrayForEach : RunFSMAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Array to iterate through.")]
		public FsmArray array;

		[MatchElementType("array")]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the item in a variable")]
		[HideTypeFilter]
		public FsmVar storeItem;

		[ActionSection("Run FSM")]
		public FsmTemplateControl fsmTemplateControl = new FsmTemplateControl();

		[Tooltip("Event to send after iterating through all items in the Array.")]
		public FsmEvent finishEvent;

		private int currentIndex;

		public override void Reset()
		{
			array = null;
			fsmTemplateControl = new FsmTemplateControl();
			runFsm = null;
		}

		public override void Awake()
		{
			if (array != null && fsmTemplateControl.fsmTemplate != null && Application.isPlaying)
			{
				runFsm = base.Fsm.CreateSubFsm(fsmTemplateControl);
			}
		}

		public override void OnEnter()
		{
			if (array == null || runFsm == null)
			{
				Finish();
				return;
			}
			currentIndex = 0;
			StartFsm();
		}

		public override void OnUpdate()
		{
			runFsm.Update();
			if (runFsm.Finished)
			{
				StartNextFsm();
			}
		}

		public override void OnFixedUpdate()
		{
			runFsm.LateUpdate();
			if (runFsm.Finished)
			{
				StartNextFsm();
			}
		}

		public override void OnLateUpdate()
		{
			runFsm.LateUpdate();
			if (runFsm.Finished)
			{
				StartNextFsm();
			}
		}

		private void StartNextFsm()
		{
			currentIndex++;
			StartFsm();
		}

		private void StartFsm()
		{
			while (currentIndex < array.Length)
			{
				DoStartFsm();
				if (!runFsm.Finished)
				{
					return;
				}
				currentIndex++;
			}
			base.Fsm.Event(finishEvent);
			Finish();
		}

		private void DoStartFsm()
		{
			storeItem.SetValue(array.Values[currentIndex]);
			fsmTemplateControl.UpdateValues();
			fsmTemplateControl.ApplyOverrides(runFsm);
			runFsm.OnEnable();
			if (!runFsm.Started)
			{
				runFsm.Start();
			}
		}

		protected override void CheckIfFinished()
		{
		}
	}
}
