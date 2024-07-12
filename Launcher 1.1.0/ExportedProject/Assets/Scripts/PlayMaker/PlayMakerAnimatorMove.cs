public class PlayMakerAnimatorMove : PlayMakerProxyBase
{
	public void OnAnimatorMove()
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleAnimatorMove)
			{
				playMakerFSM.Fsm.OnAnimatorMove();
			}
		}
	}
}
