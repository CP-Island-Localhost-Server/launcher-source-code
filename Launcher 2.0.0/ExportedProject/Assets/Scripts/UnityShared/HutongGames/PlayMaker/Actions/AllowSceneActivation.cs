namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Scene)]
	[Tooltip("Create an empty new scene with the given name additively. The path of the new scene will be empty")]
	public class AllowSceneActivation : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The name of the new scene. It cannot be empty or null, or same as the name of the existing scenes.")]
		[RequiredField]
		public FsmInt aSynchOperationHashCode;

		[Tooltip("Allow the scene to be activated as soon as it's ready")]
		public FsmBool allowSceneActivation;

		[Tooltip("useful if activation will be set during update")]
		public bool everyframe;

		[ActionSection("Result")]
		[Tooltip("The loading's progress.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat progress;

		[UIHint(UIHint.Variable)]
		[Tooltip("True when loading is done")]
		public FsmBool isDone;

		[Tooltip("Event sent when scene loading is done")]
		public FsmEvent doneEvent;

		[Tooltip("Event sent when action could not be performed. Check Log for more information")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			aSynchOperationHashCode = null;
			allowSceneActivation = null;
			everyframe = false;
			progress = null;
			isDone = null;
			doneEvent = null;
			failureEvent = null;
		}

		public override void OnEnter()
		{
			DoAllowSceneActivation();
			if (!everyframe)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoAllowSceneActivation();
		}

		private void DoAllowSceneActivation()
		{
			if (aSynchOperationHashCode.IsNone || allowSceneActivation.IsNone || LoadSceneAsynch.aSyncOperationLUT == null || !LoadSceneAsynch.aSyncOperationLUT.ContainsKey(aSynchOperationHashCode.Value))
			{
				base.Fsm.Event(failureEvent);
				Finish();
				return;
			}
			if (!progress.IsNone)
			{
				progress.Value = LoadSceneAsynch.aSyncOperationLUT[aSynchOperationHashCode.Value].progress;
			}
			if (!isDone.IsNone)
			{
				isDone.Value = LoadSceneAsynch.aSyncOperationLUT[aSynchOperationHashCode.Value].isDone;
				if (LoadSceneAsynch.aSyncOperationLUT[aSynchOperationHashCode.Value].isDone)
				{
					LoadSceneAsynch.aSyncOperationLUT.Remove(aSynchOperationHashCode.Value);
					base.Fsm.Event(doneEvent);
					Finish();
					return;
				}
			}
			LoadSceneAsynch.aSyncOperationLUT[aSynchOperationHashCode.Value].allowSceneActivation = allowSceneActivation.Value;
		}
	}
}
