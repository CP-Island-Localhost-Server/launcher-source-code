using UnityEngine;

public class PlayMakerTriggerStay2D : PlayMakerProxyBase
{
	public void OnTriggerStay2D(Collider2D other)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleTriggerStay2D)
			{
				playMakerFSM.Fsm.OnTriggerStay2D(other);
			}
		}
	}
}
