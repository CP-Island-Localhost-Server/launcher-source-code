public class PlayMakerFixedUpdate : PlayMakerProxyBase
{
	public void FixedUpdate()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleFixedUpdate)
			{
				playMakerFSM.Fsm.FixedUpdate();
			}
		}
	}
}
