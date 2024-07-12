using UnityEngine;

namespace Disney.Kelowna.Common.SEDFSM
{
	public class EnableDisableBehaviourStateHandler : PassiveStateHandler
	{
		public Behaviour[] Targets;

		public string[] EnabledStates;

		public override void HandleStateChange(string newState)
		{
			bool flag = false;
			for (int i = 0; i < EnabledStates.Length; i++)
			{
				if (EnabledStates[i] == newState)
				{
					flag = true;
					break;
				}
			}
			for (int i = 0; i < Targets.Length; i++)
			{
				Targets[i].enabled = flag;
			}
		}
	}
}
