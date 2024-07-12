using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

public class PlayMakerGlobals : ScriptableObject
{
	private static PlayMakerGlobals instance;

	[SerializeField]
	private FsmVariables variables = new FsmVariables();

	[SerializeField]
	private List<string> events = new List<string>();

	public static bool Initialized { get; private set; }

	public static bool IsPlayingInEditor { get; private set; }

	public static bool IsPlaying { get; private set; }

	public static bool IsEditor { get; private set; }

	public static bool IsBuilding { get; set; }

	public static PlayMakerGlobals Instance
	{
		get
		{
			Initialize();
			return instance;
		}
	}

	public FsmVariables Variables
	{
		get
		{
			return variables;
		}
		set
		{
			variables = value;
		}
	}

	public List<string> Events
	{
		get
		{
			return events;
		}
		set
		{
			events = value;
		}
	}

	public static void InitApplicationFlags()
	{
		IsPlayingInEditor = Application.isEditor && Application.isPlaying;
		IsPlaying = Application.isPlaying || IsBuilding;
		IsEditor = Application.isEditor;
	}

	public static void Initialize()
	{
		if (Initialized)
		{
			return;
		}
		InitApplicationFlags();
		Object @object = Resources.Load("PlayMakerGlobals", typeof(PlayMakerGlobals));
		if (@object != null)
		{
			if (IsPlayingInEditor)
			{
				PlayMakerGlobals playMakerGlobals = (PlayMakerGlobals)@object;
				instance = ScriptableObject.CreateInstance<PlayMakerGlobals>();
				instance.Variables = new FsmVariables(playMakerGlobals.variables);
				instance.Events = new List<string>(playMakerGlobals.Events);
			}
			else
			{
				instance = @object as PlayMakerGlobals;
			}
		}
		else
		{
			instance = ScriptableObject.CreateInstance<PlayMakerGlobals>();
		}
		Initialized = true;
	}

	public static void ResetInstance()
	{
		instance = null;
	}

	public FsmEvent AddEvent(string eventName)
	{
		events.Add(eventName);
		FsmEvent fsmEvent = FsmEvent.FindEvent(eventName) ?? FsmEvent.GetFsmEvent(eventName);
		fsmEvent.IsGlobal = true;
		return fsmEvent;
	}

	public void OnEnable()
	{
	}

	public void OnDestroy()
	{
		Initialized = false;
		instance = null;
	}
}
