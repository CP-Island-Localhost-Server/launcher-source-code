using System.Collections;
using DevonLocalization.Core;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public abstract class LauncherAction : MonoBehaviour
	{
		[SerializeField]
		private float loadRatio = 1f;

		[LocalizationToken]
		[SerializeField]
		private string buttonToken = null;

		protected bool canRunNextStep = true;

		public float LoadRatio
		{
			get
			{
				return loadRatio;
			}
		}

		public string ButtonToken
		{
			get
			{
				return buttonToken;
			}
		}

		public bool CanRunNextStep
		{
			get
			{
				return canRunNextStep;
			}
		}

		private void OnValidate()
		{
		}

		public abstract IEnumerator Run();

		public abstract float GetProgress();

		public virtual LauncherStatus GetLauncherStatus()
		{
			return LauncherStatus.Unchanged;
		}
	}
}
