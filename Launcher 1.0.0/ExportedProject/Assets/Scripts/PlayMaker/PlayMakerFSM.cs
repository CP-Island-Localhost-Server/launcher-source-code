using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[AddComponentMenu("PlayMaker/PlayMakerFSM")]
public class PlayMakerFSM : MonoBehaviour, ISerializationCallbackReceiver
{
	private static readonly List<PlayMakerFSM> fsmList = new List<PlayMakerFSM>();

	public static bool MaximizeFileCompatibility;

	public static bool ApplicationIsQuitting;

	public static bool NotMainThread;

	[SerializeField]
	[HideInInspector]
	private Fsm fsm;

	[SerializeField]
	private FsmTemplate fsmTemplate;

	[SerializeField]
	private bool eventHandlerComponentsAdded;

	public static string VersionNotes
	{
		get
		{
			return "";
		}
	}

	public static string VersionLabel
	{
		get
		{
			return "";
		}
	}

	public static List<PlayMakerFSM> FsmList
	{
		get
		{
			return fsmList;
		}
	}

	public FsmTemplate FsmTemplate
	{
		get
		{
			return fsmTemplate;
		}
	}

	public GUITexture GuiTexture { get; private set; }

	public GUIText GuiText { get; private set; }

	public static bool DrawGizmos { get; set; }

	public Fsm Fsm
	{
		get
		{
			fsm.Owner = this;
			return fsm;
		}
	}

	public string FsmName
	{
		get
		{
			return fsm.Name;
		}
		set
		{
			fsm.Name = value;
		}
	}

	public string FsmDescription
	{
		get
		{
			return fsm.Description;
		}
		set
		{
			fsm.Description = value;
		}
	}

	public bool Active
	{
		get
		{
			return fsm.Active;
		}
	}

	public string ActiveStateName
	{
		get
		{
			if (fsm.ActiveState != null)
			{
				return fsm.ActiveState.Name;
			}
			return "";
		}
	}

	public FsmState[] FsmStates
	{
		get
		{
			return fsm.States;
		}
	}

	public FsmEvent[] FsmEvents
	{
		get
		{
			return fsm.Events;
		}
	}

	public FsmTransition[] FsmGlobalTransitions
	{
		get
		{
			return fsm.GlobalTransitions;
		}
	}

	public FsmVariables FsmVariables
	{
		get
		{
			return fsm.Variables;
		}
	}

	public bool UsesTemplate
	{
		get
		{
			return fsmTemplate != null;
		}
	}

	public static PlayMakerFSM FindFsmOnGameObject(GameObject go, string fsmName)
	{
		foreach (PlayMakerFSM fsm in fsmList)
		{
			if (fsm.gameObject == go && fsm.FsmName == fsmName)
			{
				return fsm;
			}
		}
		return null;
	}

	public void Reset()
	{
		if (fsm == null)
		{
			fsm = new Fsm();
		}
		fsmTemplate = null;
		fsm.Reset(this);
	}

	private void Awake()
	{
		PlayMakerGlobals.Initialize();
		if (!PlayMakerGlobals.IsEditor)
		{
			FsmLog.LoggingEnabled = false;
		}
		Init();
	}

	public void Preprocess()
	{
		if (fsmTemplate != null)
		{
			InitTemplate();
		}
		else
		{
			InitFsm();
		}
		fsm.Preprocess(this);
		AddEventHandlerComponents();
	}

	private void Init()
	{
		if (fsmTemplate != null)
		{
			if (Application.isPlaying)
			{
				InitTemplate();
			}
		}
		else
		{
			InitFsm();
		}
		if (PlayMakerGlobals.IsEditor)
		{
			fsm.Preprocessed = false;
			eventHandlerComponentsAdded = false;
		}
		fsm.Init(this);
		if (!eventHandlerComponentsAdded || !fsm.Preprocessed)
		{
			AddEventHandlerComponents();
		}
	}

	private void InitTemplate()
	{
		string text = fsm.Name;
		bool enableDebugFlow = fsm.EnableDebugFlow;
		bool enableBreakpoints = fsm.EnableBreakpoints;
		bool showStateLabel = fsm.ShowStateLabel;
		fsm = new Fsm(fsmTemplate.fsm, fsm.Variables)
		{
			Name = text,
			UsedInTemplate = null,
			EnableDebugFlow = enableDebugFlow,
			EnableBreakpoints = enableBreakpoints,
			ShowStateLabel = showStateLabel
		};
	}

	private void InitFsm()
	{
		if (fsm == null)
		{
			Reset();
		}
		if (fsm == null)
		{
			Debug.LogError("Could not initialize FSM!");
			base.enabled = false;
		}
	}

	public void AddEventHandlerComponents()
	{
		if (!PlayMakerGlobals.IsEditor)
		{
			Debug.Log("FSM not Preprocessed: " + FsmUtility.GetFullFsmLabel(fsm));
		}
		if (fsm.MouseEvents)
		{
			AddEventHandlerComponent<PlayMakerMouseEvents>();
		}
		if (fsm.HandleCollisionEnter)
		{
			AddEventHandlerComponent<PlayMakerCollisionEnter>();
		}
		if (fsm.HandleCollisionExit)
		{
			AddEventHandlerComponent<PlayMakerCollisionExit>();
		}
		if (fsm.HandleCollisionStay)
		{
			AddEventHandlerComponent<PlayMakerCollisionStay>();
		}
		if (fsm.HandleTriggerEnter)
		{
			AddEventHandlerComponent<PlayMakerTriggerEnter>();
		}
		if (fsm.HandleTriggerExit)
		{
			AddEventHandlerComponent<PlayMakerTriggerExit>();
		}
		if (fsm.HandleTriggerStay)
		{
			AddEventHandlerComponent<PlayMakerTriggerStay>();
		}
		if (fsm.HandleCollisionEnter2D)
		{
			AddEventHandlerComponent<PlayMakerCollisionEnter2D>();
		}
		if (fsm.HandleCollisionExit2D)
		{
			AddEventHandlerComponent<PlayMakerCollisionExit2D>();
		}
		if (fsm.HandleCollisionStay2D)
		{
			AddEventHandlerComponent<PlayMakerCollisionStay2D>();
		}
		if (fsm.HandleTriggerEnter2D)
		{
			AddEventHandlerComponent<PlayMakerTriggerEnter2D>();
		}
		if (fsm.HandleTriggerExit2D)
		{
			AddEventHandlerComponent<PlayMakerTriggerExit2D>();
		}
		if (fsm.HandleTriggerStay2D)
		{
			AddEventHandlerComponent<PlayMakerTriggerStay2D>();
		}
		if (fsm.HandleParticleCollision)
		{
			AddEventHandlerComponent<PlayMakerParticleCollision>();
		}
		if (fsm.HandleControllerColliderHit)
		{
			AddEventHandlerComponent<PlayMakerControllerColliderHit>();
		}
		if (fsm.HandleJointBreak)
		{
			AddEventHandlerComponent<PlayMakerJointBreak>();
		}
		if (fsm.HandleJointBreak2D)
		{
			AddEventHandlerComponent<PlayMakerJointBreak>();
		}
		if (fsm.HandleFixedUpdate)
		{
			AddEventHandlerComponent<PlayMakerFixedUpdate>();
		}
		if (fsm.HandleOnGUI && GetComponent<PlayMakerOnGUI>() == null)
		{
			PlayMakerOnGUI playMakerOnGUI = base.gameObject.AddComponent<PlayMakerOnGUI>();
			playMakerOnGUI.playMakerFSM = this;
		}
		if (fsm.HandleApplicationEvents)
		{
			AddEventHandlerComponent<PlayMakerApplicationEvents>();
		}
		if (fsm.HandleAnimatorMove)
		{
			AddEventHandlerComponent<PlayMakerAnimatorMove>();
		}
		if (fsm.HandleAnimatorIK)
		{
			AddEventHandlerComponent<PlayMakerAnimatorIK>();
		}
		eventHandlerComponentsAdded = true;
	}

	private void AddEventHandlerComponent<T>(HideFlags hide = HideFlags.HideInInspector) where T : Component
	{
		if (!(GetComponent<T>() != null))
		{
			if (!PlayMakerGlobals.IsEditor)
			{
				Debug.Log("AddEventHandlerComponent: " + typeof(T));
			}
			T val = base.gameObject.AddComponent<T>();
			val.hideFlags = hide;
		}
	}

	public void SetFsmTemplate(FsmTemplate template)
	{
		fsmTemplate = template;
		Fsm.Clear(this);
		if (template != null)
		{
			Fsm.Variables = new FsmVariables(fsmTemplate.fsm.Variables);
		}
		Init();
	}

	private void Start()
	{
		GuiTexture = GetComponent<GUITexture>();
		GuiText = GetComponent<GUIText>();
		if (!fsm.Started)
		{
			fsm.Start();
		}
	}

	private void OnEnable()
	{
		fsmList.Add(this);
		fsm.OnEnable();
	}

	private void Update()
	{
		if (!fsm.Finished && !fsm.ManualUpdate)
		{
			fsm.Update();
		}
	}

	private void LateUpdate()
	{
		FsmVariables.GlobalVariablesSynced = false;
		if (!fsm.Finished)
		{
			fsm.LateUpdate();
		}
	}

	public IEnumerator DoCoroutine(IEnumerator routine)
	{
		while (true)
		{
			FsmExecutionStack.PushFsm(fsm);
			if (!routine.MoveNext())
			{
				break;
			}
			FsmExecutionStack.PopFsm();
			yield return routine.Current;
		}
		FsmExecutionStack.PopFsm();
	}

	private void OnDisable()
	{
		fsmList.Remove(this);
		if (fsm != null && !fsm.Finished)
		{
			fsm.OnDisable();
		}
	}

	private void OnDestroy()
	{
		fsmList.Remove(this);
		if (fsm != null)
		{
			fsm.OnDestroy();
		}
	}

	private void OnApplicationQuit()
	{
		fsm.Event(FsmEvent.ApplicationQuit);
		ApplicationIsQuitting = true;
	}

	private void OnDrawGizmos()
	{
		if (fsm != null)
		{
			fsm.OnDrawGizmos();
		}
	}

	public void SetState(string stateName)
	{
		fsm.SetState(stateName);
	}

	public void ChangeState(FsmEvent fsmEvent)
	{
		fsm.Event(fsmEvent);
	}

	[Obsolete("Use SendEvent(string) instead.")]
	public void ChangeState(string eventName)
	{
		fsm.Event(eventName);
	}

	public void SendEvent(string eventName)
	{
		fsm.Event(eventName);
	}

	[RPC]
	public void SendRemoteFsmEvent(string eventName)
	{
		fsm.Event(eventName);
	}

	[RPC]
	public void SendRemoteFsmEventWithData(string eventName, string eventData)
	{
		Fsm.EventData.StringData = eventData;
		fsm.Event(eventName);
	}

	public static void BroadcastEvent(string fsmEventName)
	{
		if (!string.IsNullOrEmpty(fsmEventName))
		{
			BroadcastEvent(FsmEvent.GetFsmEvent(fsmEventName));
		}
	}

	public static void BroadcastEvent(FsmEvent fsmEvent)
	{
		List<PlayMakerFSM> list = new List<PlayMakerFSM>(FsmList);
		foreach (PlayMakerFSM item in list)
		{
			if (!(item == null) && item.Fsm != null)
			{
				item.Fsm.ProcessEvent(fsmEvent);
			}
		}
	}

	private void OnBecameVisible()
	{
		fsm.Event(FsmEvent.BecameVisible);
	}

	private void OnBecameInvisible()
	{
		fsm.Event(FsmEvent.BecameInvisible);
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		Fsm.EventData.Player = player;
		fsm.Event(FsmEvent.PlayerConnected);
	}

	private void OnServerInitialized()
	{
		fsm.Event(FsmEvent.ServerInitialized);
	}

	private void OnConnectedToServer()
	{
		fsm.Event(FsmEvent.ConnectedToServer);
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Fsm.EventData.Player = player;
		fsm.Event(FsmEvent.PlayerDisconnected);
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Fsm.EventData.DisconnectionInfo = info;
		fsm.Event(FsmEvent.DisconnectedFromServer);
	}

	private void OnFailedToConnect(NetworkConnectionError error)
	{
		Fsm.EventData.ConnectionError = error;
		fsm.Event(FsmEvent.FailedToConnect);
	}

	private void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		Fsm.EventData.NetworkMessageInfo = info;
		fsm.Event(FsmEvent.NetworkInstantiate);
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!FsmVariables.GlobalVariablesSynced)
		{
			FsmVariables.GlobalVariablesSynced = true;
			NetworkSyncVariables(stream, FsmVariables.GlobalVariables);
		}
		NetworkSyncVariables(stream, Fsm.Variables);
	}

	private static void NetworkSyncVariables(BitStream stream, FsmVariables variables)
	{
		FsmInt[] intVariables;
		FsmQuaternion[] quaternionVariables;
		FsmVector3[] vector3Variables;
		FsmColor[] colorVariables;
		FsmVector2[] vector2Variables;
		if (stream.isWriting)
		{
			FsmString[] stringVariables = variables.StringVariables;
			foreach (FsmString fsmString in stringVariables)
			{
				if (fsmString.NetworkSync)
				{
					char[] array = fsmString.Value.ToCharArray();
					int value = array.Length;
					stream.Serialize(ref value);
					for (int j = 0; j < value; j++)
					{
						stream.Serialize(ref array[j]);
					}
				}
			}
			FsmBool[] boolVariables = variables.BoolVariables;
			foreach (FsmBool fsmBool in boolVariables)
			{
				if (fsmBool.NetworkSync)
				{
					bool value2 = fsmBool.Value;
					stream.Serialize(ref value2);
				}
			}
			FsmFloat[] floatVariables = variables.FloatVariables;
			foreach (FsmFloat fsmFloat in floatVariables)
			{
				if (fsmFloat.NetworkSync)
				{
					float value3 = fsmFloat.Value;
					stream.Serialize(ref value3);
				}
			}
			intVariables = variables.IntVariables;
			foreach (FsmInt fsmInt in intVariables)
			{
				if (fsmInt.NetworkSync)
				{
					int value4 = fsmInt.Value;
					stream.Serialize(ref value4);
				}
			}
			quaternionVariables = variables.QuaternionVariables;
			foreach (FsmQuaternion fsmQuaternion in quaternionVariables)
			{
				if (fsmQuaternion.NetworkSync)
				{
					Quaternion value5 = fsmQuaternion.Value;
					stream.Serialize(ref value5);
				}
			}
			vector3Variables = variables.Vector3Variables;
			foreach (FsmVector3 fsmVector in vector3Variables)
			{
				if (fsmVector.NetworkSync)
				{
					Vector3 value6 = fsmVector.Value;
					stream.Serialize(ref value6);
				}
			}
			colorVariables = variables.ColorVariables;
			foreach (FsmColor fsmColor in colorVariables)
			{
				if (fsmColor.NetworkSync)
				{
					Color value7 = fsmColor.Value;
					stream.Serialize(ref value7.r);
					stream.Serialize(ref value7.g);
					stream.Serialize(ref value7.b);
					stream.Serialize(ref value7.a);
				}
			}
			vector2Variables = variables.Vector2Variables;
			foreach (FsmVector2 fsmVector2 in vector2Variables)
			{
				if (fsmVector2.NetworkSync)
				{
					Vector2 value8 = fsmVector2.Value;
					stream.Serialize(ref value8.x);
					stream.Serialize(ref value8.y);
				}
			}
			return;
		}
		FsmString[] stringVariables2 = variables.StringVariables;
		foreach (FsmString fsmString2 in stringVariables2)
		{
			if (fsmString2.NetworkSync)
			{
				int value9 = 0;
				stream.Serialize(ref value9);
				char[] array2 = new char[value9];
				for (int num5 = 0; num5 < value9; num5++)
				{
					stream.Serialize(ref array2[num5]);
				}
				fsmString2.Value = new string(array2);
			}
		}
		FsmBool[] boolVariables2 = variables.BoolVariables;
		foreach (FsmBool fsmBool2 in boolVariables2)
		{
			if (fsmBool2.NetworkSync)
			{
				bool value10 = false;
				stream.Serialize(ref value10);
				fsmBool2.Value = value10;
			}
		}
		FsmFloat[] floatVariables2 = variables.FloatVariables;
		foreach (FsmFloat fsmFloat2 in floatVariables2)
		{
			if (fsmFloat2.NetworkSync)
			{
				float value11 = 0f;
				stream.Serialize(ref value11);
				fsmFloat2.Value = value11;
			}
		}
		intVariables = variables.IntVariables;
		foreach (FsmInt fsmInt2 in intVariables)
		{
			if (fsmInt2.NetworkSync)
			{
				int value12 = 0;
				stream.Serialize(ref value12);
				fsmInt2.Value = value12;
			}
		}
		quaternionVariables = variables.QuaternionVariables;
		foreach (FsmQuaternion fsmQuaternion2 in quaternionVariables)
		{
			if (fsmQuaternion2.NetworkSync)
			{
				Quaternion value13 = Quaternion.identity;
				stream.Serialize(ref value13);
				fsmQuaternion2.Value = value13;
			}
		}
		vector3Variables = variables.Vector3Variables;
		foreach (FsmVector3 fsmVector3 in vector3Variables)
		{
			if (fsmVector3.NetworkSync)
			{
				Vector3 value14 = Vector3.zero;
				stream.Serialize(ref value14);
				fsmVector3.Value = value14;
			}
		}
		colorVariables = variables.ColorVariables;
		foreach (FsmColor fsmColor2 in colorVariables)
		{
			if (fsmColor2.NetworkSync)
			{
				float value15 = 0f;
				stream.Serialize(ref value15);
				float value16 = 0f;
				stream.Serialize(ref value16);
				float value17 = 0f;
				stream.Serialize(ref value17);
				float value18 = 0f;
				stream.Serialize(ref value18);
				fsmColor2.Value = new Color(value15, value16, value17, value18);
			}
		}
		vector2Variables = variables.Vector2Variables;
		foreach (FsmVector2 fsmVector4 in vector2Variables)
		{
			if (fsmVector4.NetworkSync)
			{
				float value19 = 0f;
				stream.Serialize(ref value19);
				float value20 = 0f;
				stream.Serialize(ref value20);
				fsmVector4.Value = new Vector2(value19, value20);
			}
		}
	}

	private void OnMasterServerEvent(MasterServerEvent masterServerEvent)
	{
		Fsm.EventData.MasterServerEvent = masterServerEvent;
		fsm.Event(FsmEvent.MasterServerEvent);
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		NotMainThread = true;
		if (PlayMakerGlobals.Initialized)
		{
			fsm.InitData();
		}
		NotMainThread = false;
	}
}
