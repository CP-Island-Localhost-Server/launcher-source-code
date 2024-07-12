using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class LayoutOption
	{
		public enum LayoutOptionType
		{
			Width = 0,
			Height = 1,
			MinWidth = 2,
			MaxWidth = 3,
			MinHeight = 4,
			MaxHeight = 5,
			ExpandWidth = 6,
			ExpandHeight = 7
		}

		public LayoutOptionType option;

		public FsmFloat floatParam;

		public FsmBool boolParam;

		public LayoutOption()
		{
			ResetParameters();
		}

		public LayoutOption(LayoutOption source)
		{
			option = source.option;
			floatParam = new FsmFloat(source.floatParam);
			boolParam = new FsmBool(source.boolParam);
		}

		public void ResetParameters()
		{
			floatParam = 0f;
			boolParam = false;
		}

		public GUILayoutOption GetGUILayoutOption()
		{
			switch (option)
			{
			case LayoutOptionType.Width:
				return GUILayout.Width(floatParam.Value);
			case LayoutOptionType.Height:
				return GUILayout.Height(floatParam.Value);
			case LayoutOptionType.MinWidth:
				return GUILayout.MinWidth(floatParam.Value);
			case LayoutOptionType.MaxWidth:
				return GUILayout.MaxWidth(floatParam.Value);
			case LayoutOptionType.MinHeight:
				return GUILayout.MinHeight(floatParam.Value);
			case LayoutOptionType.MaxHeight:
				return GUILayout.MaxHeight(floatParam.Value);
			case LayoutOptionType.ExpandWidth:
				return GUILayout.ExpandWidth(boolParam.Value);
			case LayoutOptionType.ExpandHeight:
				return GUILayout.ExpandHeight(boolParam.Value);
			default:
				return null;
			}
		}
	}
}
