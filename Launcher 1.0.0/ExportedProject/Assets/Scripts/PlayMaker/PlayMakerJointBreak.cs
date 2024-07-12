public class PlayMakerJointBreak : PlayMakerProxyBase
{
	public void OnJointBreak(float breakForce)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleJointBreak)
			{
				playMakerFSM.Fsm.OnJointBreak(breakForce);
			}
		}
	}
}
