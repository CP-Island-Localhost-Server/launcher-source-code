using UnityEngine;

public class PlayMakerCollisionExit2D : PlayMakerProxyBase
{
	public void OnCollisionExit2D(Collision2D collisionInfo)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleCollisionExit2D)
			{
				playMakerFSM.Fsm.OnCollisionExit2D(collisionInfo);
			}
		}
	}
}
