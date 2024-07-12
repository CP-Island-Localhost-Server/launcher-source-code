using UnityEngine;

public class PlayMakerControllerColliderHit : PlayMakerProxyBase
{
	public void OnControllerColliderHit(ControllerColliderHit hitCollider)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleControllerColliderHit)
			{
				playMakerFSM.Fsm.OnControllerColliderHit(hitCollider);
			}
		}
	}
}
