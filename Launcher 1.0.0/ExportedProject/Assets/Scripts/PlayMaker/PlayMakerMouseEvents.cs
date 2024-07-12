using HutongGames.PlayMaker;

public class PlayMakerMouseEvents : PlayMakerProxyBase
{
	public void OnMouseEnter()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseEnter);
			}
		}
	}

	public void OnMouseDown()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseDown);
			}
		}
	}

	public void OnMouseUp()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseUp);
				Fsm.LastClickedObject = base.gameObject;
			}
		}
	}

	public void OnMouseUpAsButton()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseUpAsButton);
				Fsm.LastClickedObject = base.gameObject;
			}
		}
	}

	public void OnMouseExit()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseExit);
			}
		}
	}

	public void OnMouseDrag()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseDrag);
			}
		}
	}

	public void OnMouseOver()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseOver);
			}
		}
	}
}
