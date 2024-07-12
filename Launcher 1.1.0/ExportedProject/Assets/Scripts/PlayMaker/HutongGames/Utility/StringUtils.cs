using System.Text.RegularExpressions;

namespace HutongGames.Utility
{
	public static class StringUtils
	{
		public static string IncrementStringCounter(string s)
		{
			Match match = Regex.Match(s, "(?<Name>.*)\\s(?<Number>[0-9]+)$");
			if (!match.Success)
			{
				return s + " 2";
			}
			int length = match.Groups["Number"].Length;
			int result;
			if (!int.TryParse(match.Groups["Number"].Value, out result))
			{
				result = 1;
			}
			return match.Groups["Name"].Value + " " + (result + 1).ToString("D" + length);
		}
	}
}
