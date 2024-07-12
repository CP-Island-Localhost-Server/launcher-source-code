using UnityEngine;

public class PlayMakerCollisionExit : PlayMakerProxyBase
{
	public void OnCollisionExit(Collision collisionInfo)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleCollisionExit)
			{
				playMakerFSM.Fsm.OnCollisionExit(collisionInfo);
			}
		}
	}
}
