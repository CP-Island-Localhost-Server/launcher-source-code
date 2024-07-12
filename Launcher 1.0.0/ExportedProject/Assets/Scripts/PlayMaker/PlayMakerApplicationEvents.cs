using HutongGames.PlayMaker;

public class PlayMakerApplicationEvents : PlayMakerProxyBase
{
	public void OnApplicationFocus()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.HandleApplicationEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.ApplicationFocus);
			}
		}
	}

	public void OnApplicationPause()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Fsm.HandleApplicationEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.ApplicationPause);
			}
		}
	}
}
