using UnityEngine;

public class PlayMakerTriggerExit : PlayMakerProxyBase
{
	public void OnTriggerExit(Collider other)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleTriggerExit)
			{
				playMakerFSM.Fsm.OnTriggerExit(other);
			}
		}
	}
}
