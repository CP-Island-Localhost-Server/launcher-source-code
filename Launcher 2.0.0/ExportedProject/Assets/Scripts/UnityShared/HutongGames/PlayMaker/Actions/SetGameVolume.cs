using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the global sound volume.")]
	[ActionCategory(ActionCategory.Audio)]
	public class SetGameVolume : FsmStateAction
	{
		[HasFloatSlider(0f, 1f)]
		[RequiredField]
		public FsmFloat volume;

		public bool everyFrame;

		public override void Reset()
		{
			volume = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			AudioListener.volume = volume.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			AudioListener.volume = volume.Value;
		}
	}
}
