using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Sets the global sound volume.")]
	public class SetGameVolume : FsmStateAction
	{
		[RequiredField]
		[HasFloatSlider(0f, 1f)]
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
