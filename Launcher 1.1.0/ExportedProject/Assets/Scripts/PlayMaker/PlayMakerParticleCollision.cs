using UnityEngine;

public class PlayMakerParticleCollision : PlayMakerProxyBase
{
	public void OnParticleCollision(GameObject other)
	{
		for (int i = 0; i < playMakerFSMs.Length; i++)
		{
			PlayMakerFSM playMakerFSM = playMakerFSMs[i];
			if (playMakerFSM.Active && playMakerFSM.Fsm.HandleParticleCollision)
			{
				playMakerFSM.Fsm.OnParticleCollision(other);
			}
		}
	}
}
