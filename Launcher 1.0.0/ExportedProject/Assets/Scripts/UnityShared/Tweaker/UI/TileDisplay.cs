using System.Text;

namespace Tweaker.UI
{
	public static class TileDisplay
	{
		public static string GetFriendlyName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(name.Length, name.Length * 2);
			stringBuilder.Append(name[0]);
			for (int i = 1; i < name.Length; i++)
			{
				if (char.IsUpper(name[i]) && !char.IsUpper(name[i - 1]))
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(name[i]);
			}
			return stringBuilder.ToString();
		}
	}
}
