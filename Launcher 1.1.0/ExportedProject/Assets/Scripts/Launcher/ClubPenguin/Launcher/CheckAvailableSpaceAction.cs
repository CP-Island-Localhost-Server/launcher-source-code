using System.Collections;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class CheckAvailableSpaceAction : LauncherAction
	{
		public float OSXRequiredSpaceGB;

		public float WindowsInstallerRequiredSpaceGB;

		public float WindowsAppRequiredSpaceGB;

		[LocalizationToken]
		public string PromptTitleToken;

		[LocalizationToken]
		public string PromptBodyToken;

		public override float GetProgress()
		{
			return 0f;
		}

		public override IEnumerator Run()
		{
			canRunNextStep = true;
			float num = WindowsInstallerRequiredSpaceGB + WindowsAppRequiredSpaceGB;
			bool flag;
			if (DiskUtils.IsDefaultDrive(Application.dataPath))
			{
				float num2 = (float)DiskUtils.GetAvailableSpace() / 1024f;
				flag = num2 < num;
			}
			else
			{
				float num3 = (float)DiskUtils.GetAvailableSpace() / 1024f;
				float num4 = (float)DiskUtils.GetAvailableSpace(Application.dataPath) / 1024f;
				flag = num3 < WindowsInstallerRequiredSpaceGB || num4 < WindowsAppRequiredSpaceGB;
			}
			if (flag)
			{
				canRunNextStep = false;
				string tokenTranslation = Service.Get<Localizer>().GetTokenTranslation(PromptBodyToken);
				tokenTranslation = string.Format(tokenTranslation, num);
				PromptData promptData = new PromptData(PromptTitleToken, tokenTranslation, ButtonFlags.Ok, onPromptButtonClicked);
				promptData.IsBodyTranslated = true;
				Service.Get<ICPSwrveService>().Action("error_prompt", "insufficient_space");
				Service.Get<LauncherPromptManager>().ShowError(promptData);
			}
			yield break;
		}

		private void onPromptButtonClicked(ButtonFlags clickedButtonFlag)
		{
			if (clickedButtonFlag == ButtonFlags.Ok)
			{
				QuitHelper.Quit();
			}
		}
	}
}
