using UnityEngine;

public class PlayMakerTriggerEnter : PlayMakerProxyBase
{
	public void OnTriggerEnter(Collider other)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleTriggerEnter)
			{
				playMakerFSM.Fsm.OnTriggerEnter(other);
			}
		}
	}
}
