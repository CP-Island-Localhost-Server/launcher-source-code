using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[AddComponentMenu("PlayMaker/PlayMakerGUI")]
[ExecuteInEditMode]
public class PlayMakerGUI : MonoBehaviour
{
	private const float MaxLabelWidth = 200f;

	private static readonly List<PlayMakerFSM> fsmList = new List<PlayMakerFSM>();

	public static Fsm SelectedFSM;

	private static readonly GUIContent labelContent = new GUIContent();

	public bool previewOnGUI = true;

	public bool enableGUILayout = true;

	public bool drawStateLabels = true;

	public bool enableStateLabelsInBuilds;

	public bool GUITextureStateLabels = true;

	public bool GUITextStateLabels = true;

	public bool filterLabelsWithDistance;

	public float maxLabelDistance = 10f;

	public bool controlMouseCursor = true;

	public float labelScale = 1f;

	private static readonly List<PlayMakerFSM> SortedFsmList = new List<PlayMakerFSM>();

	private static GameObject labelGameObject;

	private static float fsmLabelIndex;

	private static PlayMakerGUI instance;

	private static GUISkin guiSkin;

	private static Color guiColor = Color.white;

	private static Color guiBackgroundColor = Color.white;

	private static Color guiContentColor = Color.white;

	private static Matrix4x4 guiMatrix = Matrix4x4.identity;

	private static GUIStyle stateLabelStyle;

	private static Texture2D stateLabelBackground;

	private float initLabelScale;

	public static bool EnableStateLabels
	{
		get
		{
			InitInstance();
			if (Application.isEditor)
			{
				if (instance != null && instance.enabled)
				{
					return instance.drawStateLabels;
				}
				return false;
			}
			if (instance != null && instance.enabled && instance.drawStateLabels)
			{
				return instance.enableStateLabelsInBuilds;
			}
			return false;
		}
		set
		{
			InitInstance();
			if (instance != null)
			{
				instance.drawStateLabels = value;
			}
		}
	}

	public static bool EnableStateLabelsInBuild
	{
		get
		{
			InitInstance();
			if (instance != null && instance.enabled)
			{
				return instance.enableStateLabelsInBuilds;
			}
			return false;
		}
		set
		{
			InitInstance();
			if (instance != null)
			{
				instance.enableStateLabelsInBuilds = value;
			}
		}
	}

	public static PlayMakerGUI Instance
	{
		get
		{
			InitInstance();
			if (instance == null)
			{
				GameObject gameObject = new GameObject("PlayMakerGUI");
				instance = gameObject.AddComponent<PlayMakerGUI>();
			}
			return instance;
		}
	}

	public static bool Enabled
	{
		get
		{
			if (instance != null)
			{
				return instance.enabled;
			}
			return false;
		}
	}

	public static GUISkin GUISkin
	{
		get
		{
			return guiSkin;
		}
		set
		{
			guiSkin = value;
		}
	}

	public static Color GUIColor
	{
		get
		{
			return guiColor;
		}
		set
		{
			guiColor = value;
		}
	}

	public static Color GUIBackgroundColor
	{
		get
		{
			return guiBackgroundColor;
		}
		set
		{
			guiBackgroundColor = value;
		}
	}

	public static Color GUIContentColor
	{
		get
		{
			return guiContentColor;
		}
		set
		{
			guiContentColor = value;
		}
	}

	public static Matrix4x4 GUIMatrix
	{
		get
		{
			return guiMatrix;
		}
		set
		{
			guiMatrix = value;
		}
	}

	public static Texture MouseCursor { get; set; }

	public static bool LockCursor { get; set; }

	public static bool HideCursor { get; set; }

	private static void InitInstance()
	{
		if (instance == null)
		{
			instance = (PlayMakerGUI)UnityEngine.Object.FindObjectOfType(typeof(PlayMakerGUI));
		}
	}

	private void InitLabelStyle()
	{
		if (stateLabelBackground != null)
		{
			UnityEngine.Object.Destroy(stateLabelBackground);
		}
		stateLabelBackground = new Texture2D(1, 1);
		stateLabelBackground.SetPixel(0, 0, Color.white);
		stateLabelBackground.Apply();
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = stateLabelBackground;
		gUIStyle.normal.textColor = Color.white;
		gUIStyle.fontSize = (int)(10f * labelScale);
		gUIStyle.alignment = TextAnchor.MiddleLeft;
		gUIStyle.padding = new RectOffset(4, 4, 1, 1);
		stateLabelStyle = gUIStyle;
		initLabelScale = labelScale;
	}

	private void DrawStateLabels()
	{
		SortedFsmList.Clear();
		int count = PlayMakerFSM.FsmList.Count;
		for (int i = 0; i < count; i++)
		{
			PlayMakerFSM playMakerFSM = PlayMakerFSM.FsmList[i];
			if (playMakerFSM.Active)
			{
				SortedFsmList.Add(playMakerFSM);
			}
		}
		SortedFsmList.Sort((PlayMakerFSM x, PlayMakerFSM y) => string.CompareOrdinal(x.gameObject.name, y.gameObject.name));
		labelGameObject = null;
		count = SortedFsmList.Count;
		for (int j = 0; j < count; j++)
		{
			PlayMakerFSM playMakerFSM2 = SortedFsmList[j];
			if (playMakerFSM2.Fsm.ShowStateLabel)
			{
				DrawStateLabel(playMakerFSM2);
			}
		}
	}

	private void DrawStateLabel(PlayMakerFSM fsm)
	{
		if (stateLabelStyle == null || Math.Abs(initLabelScale - labelScale) > 0.1f)
		{
			InitLabelStyle();
		}
		if (Camera.main == null || fsm.gameObject == Camera.main)
		{
			return;
		}
		if (fsm.gameObject == labelGameObject)
		{
			fsmLabelIndex += 1f;
		}
		else
		{
			fsmLabelIndex = 0f;
			labelGameObject = fsm.gameObject;
		}
		string text = GenerateStateLabel(fsm);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Vector2 vector = default(Vector2);
		labelContent.text = text;
		Vector2 vector2 = stateLabelStyle.CalcSize(labelContent);
		vector2.x = Mathf.Clamp(vector2.x, 10f * labelScale, 200f * labelScale);
		if (GUITextureStateLabels && fsm.GuiTexture != null)
		{
			vector.x = fsm.gameObject.transform.position.x * (float)Screen.width + fsm.GuiTexture.pixelInset.x;
			vector.y = fsm.gameObject.transform.position.y * (float)Screen.height + fsm.GuiTexture.pixelInset.y;
		}
		else if (GUITextStateLabels && fsm.GuiText != null)
		{
			vector.x = fsm.gameObject.transform.position.x * (float)Screen.width;
			vector.y = fsm.gameObject.transform.position.y * (float)Screen.height;
		}
		else
		{
			if (filterLabelsWithDistance)
			{
				float num = Vector3.Distance(Camera.main.transform.position, fsm.transform.position);
				if (num > maxLabelDistance)
				{
					return;
				}
			}
			if (Camera.main.transform.InverseTransformPoint(fsm.transform.position).z <= 0f)
			{
				return;
			}
			vector = Camera.main.WorldToScreenPoint(fsm.transform.position);
			vector.x -= vector2.x * 0.5f;
		}
		vector.y = (float)Screen.height - vector.y - fsmLabelIndex * 15f * labelScale;
		Color backgroundColor = GUI.backgroundColor;
		Color color = GUI.color;
		int num2 = 0;
		if (fsm.Fsm.ActiveState != null)
		{
			num2 = fsm.Fsm.ActiveState.ColorIndex;
		}
		Color color2 = PlayMakerPrefs.Colors[num2];
		GUI.backgroundColor = new Color(color2.r, color2.g, color2.b, 0.5f);
		GUI.contentColor = Color.white;
		GUI.Label(new Rect(vector.x, vector.y, vector2.x, vector2.y), text, stateLabelStyle);
		GUI.backgroundColor = backgroundColor;
		GUI.color = color;
	}

	private static string GenerateStateLabel(PlayMakerFSM fsm)
	{
		if (fsm.Fsm.ActiveState == null)
		{
			return "[DISABLED]";
		}
		return fsm.Fsm.ActiveState.Name;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.LogWarning("There should only be one PlayMakerGUI per scene!");
		}
	}

	private void OnEnable()
	{
	}

	private void OnGUI()
	{
		base.useGUILayout = enableGUILayout;
		if (GUISkin != null)
		{
			GUI.skin = GUISkin;
		}
		GUI.color = GUIColor;
		GUI.backgroundColor = GUIBackgroundColor;
		GUI.contentColor = GUIContentColor;
		if (previewOnGUI && !Application.isPlaying)
		{
			DoEditGUI();
			return;
		}
		fsmList.Clear();
		fsmList.AddRange(PlayMakerFSM.FsmList);
		for (int i = 0; i < fsmList.Count; i++)
		{
			PlayMakerFSM playMakerFSM = fsmList[i];
			if (!(playMakerFSM == null) && playMakerFSM.Active && playMakerFSM.Fsm.ActiveState != null && !playMakerFSM.Fsm.HandleOnGUI)
			{
				CallOnGUI(playMakerFSM.Fsm);
				for (int j = 0; j < playMakerFSM.Fsm.SubFsmList.Count; j++)
				{
					Fsm fsm = playMakerFSM.Fsm.SubFsmList[j];
					CallOnGUI(fsm);
				}
			}
		}
		if (Application.isPlaying && Event.current.type == EventType.Repaint)
		{
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			if (MouseCursor != null)
			{
				Rect position = new Rect(Input.mousePosition.x - (float)MouseCursor.width * 0.5f, (float)Screen.height - Input.mousePosition.y - (float)MouseCursor.height * 0.5f, MouseCursor.width, MouseCursor.height);
				GUI.DrawTexture(position, MouseCursor);
			}
			if (drawStateLabels && EnableStateLabels)
			{
				DrawStateLabels();
			}
			GUI.matrix = matrix;
			GUIMatrix = Matrix4x4.identity;
			if (controlMouseCursor)
			{
				Cursor.lockState = (LockCursor ? CursorLockMode.Locked : CursorLockMode.None);
				Cursor.visible = !HideCursor;
			}
		}
	}

	private void CallOnGUI(Fsm fsm)
	{
		if (fsm.ActiveState == null)
		{
			return;
		}
		FsmStateAction[] actions = fsm.ActiveState.Actions;
		FsmStateAction[] array = actions;
		foreach (FsmStateAction fsmStateAction in array)
		{
			if (fsmStateAction.Active)
			{
				fsmStateAction.OnGUI();
			}
		}
	}

	private void OnDisable()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	private static void DoEditGUI()
	{
		if (SelectedFSM == null || SelectedFSM.HandleOnGUI)
		{
			return;
		}
		FsmState editState = SelectedFSM.EditState;
		if (editState == null || !editState.IsInitialized)
		{
			return;
		}
		FsmStateAction[] actions = editState.Actions;
		FsmStateAction[] array = actions;
		foreach (FsmStateAction fsmStateAction in array)
		{
			if (fsmStateAction.Enabled)
			{
				fsmStateAction.OnGUI();
			}
		}
	}

	public void OnApplicationQuit()
	{
		instance = null;
	}
}
