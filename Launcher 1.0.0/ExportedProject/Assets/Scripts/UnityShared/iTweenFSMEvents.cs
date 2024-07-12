using HutongGames.PlayMaker.Actions;
using UnityEngine;

public class iTweenFSMEvents : MonoBehaviour
{
	public static int itweenIDCount = 0;

	public int itweenID = 0;

	public iTweenFsmAction itweenFSMAction = null;

	public bool donotfinish = false;

	public bool islooping = false;

	private void iTweenOnStart(int aniTweenID)
	{
		if (itweenID == aniTweenID)
		{
			itweenFSMAction.Fsm.Event(itweenFSMAction.startEvent);
		}
	}

	private void iTweenOnComplete(int aniTweenID)
	{
		if (itweenID != aniTweenID)
		{
			return;
		}
		if (islooping)
		{
			if (!donotfinish)
			{
				itweenFSMAction.Fsm.Event(itweenFSMAction.finishEvent);
				itweenFSMAction.Finish();
			}
		}
		else
		{
			itweenFSMAction.Fsm.Event(itweenFSMAction.finishEvent);
			itweenFSMAction.Finish();
		}
	}
}
