using UnityEngine;

public class PlayMakerTriggerStay : PlayMakerProxyBase
{
	public void OnTriggerStay(Collider other)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleTriggerStay)
			{
				playMakerFSM.Fsm.OnTriggerStay(other);
			}
		}
	}
}
