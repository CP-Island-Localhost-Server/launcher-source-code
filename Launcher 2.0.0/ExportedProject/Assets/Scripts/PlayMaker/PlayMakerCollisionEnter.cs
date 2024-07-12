using UnityEngine;

public class PlayMakerCollisionEnter : PlayMakerProxyBase
{
	public void OnCollisionEnter(Collision collisionInfo)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleCollisionEnter)
			{
				playMakerFSM.Fsm.OnCollisionEnter(collisionInfo);
			}
		}
	}
}
