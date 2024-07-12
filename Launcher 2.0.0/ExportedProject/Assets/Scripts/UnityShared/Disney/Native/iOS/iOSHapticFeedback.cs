using UnityEngine;

namespace Disney.Native.iOS
{
	public class iOSHapticFeedback : MonoBehaviour
	{
		public enum ImpactFeedbackStyle
		{
			Light = 0,
			Medium = 1,
			Heavy = 2
		}

		public enum NotificationFeedbackType
		{
			Error = 0,
			Success = 1,
			Warning = 2
		}

		public enum HapticFeedbackType
		{
			None = 0,
			ImpactLight = 1,
			ImpactMedium = 2,
			ImpactHeavy = 3,
			NotificationError = 4,
			NotificationSuccess = 5,
			NotificationWarning = 6,
			Selection = 7
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		public void TriggerImpactFeedback(ImpactFeedbackStyle style)
		{
		}

		public void TriggerNotificationFeedback(NotificationFeedbackType type)
		{
		}

		public void TriggerSelectionFeedback()
		{
		}

		public void TriggerHapticFeedback(HapticFeedbackType hapticFeedback)
		{
			switch (hapticFeedback)
			{
			case HapticFeedbackType.ImpactLight:
				TriggerImpactFeedback(ImpactFeedbackStyle.Light);
				break;
			case HapticFeedbackType.ImpactMedium:
				TriggerImpactFeedback(ImpactFeedbackStyle.Medium);
				break;
			case HapticFeedbackType.ImpactHeavy:
				TriggerImpactFeedback(ImpactFeedbackStyle.Heavy);
				break;
			case HapticFeedbackType.NotificationError:
				TriggerNotificationFeedback(NotificationFeedbackType.Error);
				break;
			case HapticFeedbackType.NotificationSuccess:
				TriggerNotificationFeedback(NotificationFeedbackType.Success);
				break;
			case HapticFeedbackType.NotificationWarning:
				TriggerNotificationFeedback(NotificationFeedbackType.Warning);
				break;
			case HapticFeedbackType.Selection:
				TriggerSelectionFeedback();
				break;
			}
		}
	}
}
