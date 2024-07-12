using UnityEngine;

public class PlayMakerTriggerExit2D : PlayMakerProxyBase
{
	public void OnTriggerExit2D(Collider2D other)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleTriggerExit2D)
			{
				playMakerFSM.Fsm.OnTriggerExit2D(other);
			}
		}
	}
}
