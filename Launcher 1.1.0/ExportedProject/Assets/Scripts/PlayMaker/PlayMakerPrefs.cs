using UnityEngine;

public class PlayMakerPrefs : ScriptableObject
{
	private static PlayMakerPrefs instance;

	private static readonly Color[] defaultColors = new Color[8]
	{
		Color.grey,
		new Color(0.54509807f, 57f / 85f, 0.9411765f),
		new Color(0.24313726f, 0.7607843f, 0.6901961f),
		new Color(22f / 51f, 0.7607843f, 0.24313726f),
		new Color(1f, 0.8745098f, 16f / 85f),
		new Color(1f, 47f / 85f, 16f / 85f),
		new Color(0.7607843f, 0.24313726f, 0.2509804f),
		new Color(0.54509807f, 0.24313726f, 0.7607843f)
	};

	private static readonly string[] defaultColorNames = new string[8] { "Default", "Blue", "Cyan", "Green", "Yellow", "Orange", "Red", "Purple" };

	[SerializeField]
	private Color[] colors = new Color[24]
	{
		Color.grey,
		new Color(0.54509807f, 57f / 85f, 0.9411765f),
		new Color(0.24313726f, 0.7607843f, 0.6901961f),
		new Color(22f / 51f, 0.7607843f, 0.24313726f),
		new Color(1f, 0.8745098f, 16f / 85f),
		new Color(1f, 47f / 85f, 16f / 85f),
		new Color(0.7607843f, 0.24313726f, 0.2509804f),
		new Color(0.54509807f, 0.24313726f, 0.7607843f),
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey,
		Color.grey
	};

	[SerializeField]
	private string[] colorNames = new string[24]
	{
		"Default", "Blue", "Cyan", "Green", "Yellow", "Orange", "Red", "Purple", "", "",
		"", "", "", "", "", "", "", "", "", "",
		"", "", "", ""
	};

	private static Color[] minimapColors;

	public static PlayMakerPrefs Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load("PlayMakerPrefs") as PlayMakerPrefs;
				if (instance == null)
				{
					instance = ScriptableObject.CreateInstance<PlayMakerPrefs>();
				}
			}
			return instance;
		}
	}

	public static Color[] Colors
	{
		get
		{
			return Instance.colors;
		}
		set
		{
			Instance.colors = value;
		}
	}

	public static string[] ColorNames
	{
		get
		{
			return Instance.colorNames;
		}
		set
		{
			Instance.colorNames = value;
		}
	}

	public static Color[] MinimapColors
	{
		get
		{
			if (minimapColors == null)
			{
				UpdateMinimapColors();
			}
			return minimapColors;
		}
	}

	public void ResetDefaultColors()
	{
		for (int i = 0; i < defaultColors.Length; i++)
		{
			colors[i] = defaultColors[i];
			colorNames[i] = defaultColorNames[i];
		}
	}

	public static void SaveChanges()
	{
		UpdateMinimapColors();
	}

	private static void UpdateMinimapColors()
	{
		minimapColors = new Color[Colors.Length];
		for (int i = 0; i < Colors.Length; i++)
		{
			Color color = Colors[i];
			minimapColors[i] = new Color(color.r, color.g, color.b, 0.5f);
		}
	}
}
