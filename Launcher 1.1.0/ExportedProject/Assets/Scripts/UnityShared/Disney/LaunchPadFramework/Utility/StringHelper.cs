namespace Disney.LaunchPadFramework.Utility
{
	public static class StringHelper
	{
		private static string numberToStringFormatCode = "N";

		private static string numberToStringFloatPrecisionCode = "2";

		private static string numberToStringIntPrecisionCode = "0";

		public static string GetFloatAsFormattedString(float amount)
		{
			string text = numberToStringFormatCode;
			text = ((!(amount % 1f > 0f)) ? (text + numberToStringIntPrecisionCode) : (text + numberToStringFloatPrecisionCode));
			return amount.ToString(text);
		}
	}
}
