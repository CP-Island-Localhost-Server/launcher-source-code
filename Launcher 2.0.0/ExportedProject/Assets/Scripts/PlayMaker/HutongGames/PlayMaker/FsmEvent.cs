using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmEvent : IComparable, INameable
	{
		private static List<FsmEvent> eventList;

		private static readonly object syncObj = new object();

		[SerializeField]
		private string name;

		[SerializeField]
		private bool isSystemEvent;

		[SerializeField]
		private bool isGlobal;

		public static PlayMakerGlobals GlobalsComponent
		{
			get
			{
				return PlayMakerGlobals.Instance;
			}
		}

		public static List<string> globalEvents
		{
			get
			{
				return PlayMakerGlobals.Instance.Events;
			}
		}

		public static List<FsmEvent> EventList
		{
			get
			{
				if (eventList == null)
				{
					Initialize();
				}
				return eventList;
			}
			private set
			{
				eventList = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public bool IsSystemEvent
		{
			get
			{
				return isSystemEvent;
			}
			set
			{
				isSystemEvent = value;
			}
		}

		public bool IsMouseEvent
		{
			get
			{
				if (this != MouseDown && this != MouseDrag && this != MouseEnter && this != MouseExit && this != MouseOver && this != MouseUp)
				{
					return this == MouseUpAsButton;
				}
				return true;
			}
		}

		public bool IsApplicationEvent
		{
			get
			{
				if (this != ApplicationFocus)
				{
					return this == ApplicationPause;
				}
				return true;
			}
		}

		public bool IsGlobal
		{
			get
			{
				return isGlobal;
			}
			set
			{
				if (value)
				{
					if (!globalEvents.Contains(name))
					{
						globalEvents.Add(name);
					}
				}
				else
				{
					globalEvents.RemoveAll((string m) => m == name);
				}
				isGlobal = value;
				SanityCheckEventList();
			}
		}

		public string Path { get; set; }

		public static FsmEvent BecameInvisible { get; private set; }

		public static FsmEvent BecameVisible { get; private set; }

		public static FsmEvent CollisionEnter { get; private set; }

		public static FsmEvent CollisionExit { get; private set; }

		public static FsmEvent CollisionStay { get; private set; }

		public static FsmEvent CollisionEnter2D { get; private set; }

		public static FsmEvent CollisionExit2D { get; private set; }

		public static FsmEvent CollisionStay2D { get; private set; }

		public static FsmEvent ControllerColliderHit { get; private set; }

		public static FsmEvent Finished { get; private set; }

		public static FsmEvent LevelLoaded { get; private set; }

		public static FsmEvent MouseDown { get; private set; }

		public static FsmEvent MouseDrag { get; private set; }

		public static FsmEvent MouseEnter { get; private set; }

		public static FsmEvent MouseExit { get; private set; }

		public static FsmEvent MouseOver { get; private set; }

		public static FsmEvent MouseUp { get; private set; }

		public static FsmEvent MouseUpAsButton { get; private set; }

		public static FsmEvent TriggerEnter { get; private set; }

		public static FsmEvent TriggerExit { get; private set; }

		public static FsmEvent TriggerStay { get; private set; }

		public static FsmEvent TriggerEnter2D { get; private set; }

		public static FsmEvent TriggerExit2D { get; private set; }

		public static FsmEvent TriggerStay2D { get; private set; }

		public static FsmEvent ApplicationFocus { get; private set; }

		public static FsmEvent ApplicationPause { get; private set; }

		public static FsmEvent ApplicationQuit { get; private set; }

		public static FsmEvent ParticleCollision { get; private set; }

		public static FsmEvent JointBreak { get; private set; }

		public static FsmEvent JointBreak2D { get; private set; }

		public static FsmEvent PlayerConnected { get; private set; }

		public static FsmEvent ServerInitialized { get; private set; }

		public static FsmEvent ConnectedToServer { get; private set; }

		public static FsmEvent PlayerDisconnected { get; private set; }

		public static FsmEvent DisconnectedFromServer { get; private set; }

		public static FsmEvent FailedToConnect { get; private set; }

		public static FsmEvent FailedToConnectToMasterServer { get; private set; }

		public static FsmEvent MasterServerEvent { get; private set; }

		public static FsmEvent NetworkInstantiate { get; private set; }

		private static void Initialize()
		{
			PlayMakerGlobals.Initialize();
			eventList = new List<FsmEvent>();
			AddSystemEvents();
			AddGlobalEvents();
		}

		public static bool IsNullOrEmpty(FsmEvent fsmEvent)
		{
			if (fsmEvent != null)
			{
				return string.IsNullOrEmpty(fsmEvent.name);
			}
			return true;
		}

		public FsmEvent(string name)
		{
			lock (syncObj)
			{
				this.name = name;
				if (!EventListContainsEvent(EventList, name))
				{
					EventList.Add(this);
				}
			}
		}

		public FsmEvent(FsmEvent source)
		{
			lock (syncObj)
			{
				name = source.name;
				isSystemEvent = source.isSystemEvent;
				isGlobal = source.isGlobal;
				FsmEvent fsmEvent = EventList.Find((FsmEvent x) => x.name == name);
				if (fsmEvent == null)
				{
					EventList.Add(this);
				}
				else
				{
					fsmEvent.isGlobal |= isGlobal;
				}
			}
		}

		int IComparable.CompareTo(object obj)
		{
			FsmEvent fsmEvent = (FsmEvent)obj;
			if (isSystemEvent && !fsmEvent.isSystemEvent)
			{
				return -1;
			}
			if (!isSystemEvent && fsmEvent.isSystemEvent)
			{
				return 1;
			}
			return string.CompareOrdinal(name, fsmEvent.name);
		}

		public static bool EventListContainsEvent(List<FsmEvent> fsmEventList, string fsmEventName)
		{
			lock (syncObj)
			{
				if (fsmEventList == null || string.IsNullOrEmpty(fsmEventName))
				{
					return false;
				}
				for (int i = 0; i < fsmEventList.Count; i++)
				{
					if (fsmEventList[i].Name == fsmEventName)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static void RemoveEventFromEventList(FsmEvent fsmEvent)
		{
			if (fsmEvent.isSystemEvent)
			{
				Debug.LogError("RemoveEventFromEventList: Trying to delete System Event: " + fsmEvent.Name);
			}
			EventList.Remove(fsmEvent);
		}

		public static FsmEvent FindEvent(string eventName)
		{
			lock (syncObj)
			{
				for (int i = 0; i < EventList.Count; i++)
				{
					FsmEvent fsmEvent = EventList[i];
					if (fsmEvent.name == eventName)
					{
						return fsmEvent;
					}
				}
				return null;
			}
		}

		public static bool IsEventGlobal(string eventName)
		{
			return globalEvents.Contains(eventName);
		}

		public static bool EventListContains(string eventName)
		{
			return FindEvent(eventName) != null;
		}

		public static FsmEvent GetFsmEvent(string eventName)
		{
			lock (syncObj)
			{
				for (int i = 0; i < EventList.Count; i++)
				{
					FsmEvent fsmEvent = EventList[i];
					if (string.CompareOrdinal(fsmEvent.Name, eventName) == 0)
					{
						return PlayMakerGlobals.IsPlaying ? fsmEvent : new FsmEvent(fsmEvent);
					}
				}
				FsmEvent fsmEvent2 = new FsmEvent(eventName);
				return PlayMakerGlobals.IsPlaying ? fsmEvent2 : new FsmEvent(fsmEvent2);
			}
		}

		public static FsmEvent GetFsmEvent(FsmEvent fsmEvent)
		{
			if (fsmEvent == null)
			{
				return null;
			}
			lock (syncObj)
			{
				for (int i = 0; i < EventList.Count; i++)
				{
					FsmEvent fsmEvent2 = EventList[i];
					if (string.CompareOrdinal(fsmEvent2.Name, fsmEvent.Name) == 0)
					{
						return PlayMakerGlobals.IsPlaying ? fsmEvent2 : new FsmEvent(fsmEvent);
					}
				}
				if (fsmEvent.isSystemEvent)
				{
					Debug.LogError("Missing System Event: " + fsmEvent.Name);
				}
				return AddFsmEvent(fsmEvent);
			}
		}

		public static FsmEvent AddFsmEvent(FsmEvent fsmEvent)
		{
			EventList.Add(fsmEvent);
			return fsmEvent;
		}

		private static void AddSystemEvents()
		{
			Finished = AddSystemEvent("FINISHED", "System Events");
			BecameInvisible = AddSystemEvent("BECAME INVISIBLE", "System Events");
			BecameVisible = AddSystemEvent("BECAME VISIBLE", "System Events");
			LevelLoaded = AddSystemEvent("LEVEL LOADED", "System Events");
			MouseDown = AddSystemEvent("MOUSE DOWN", "System Events");
			MouseDrag = AddSystemEvent("MOUSE DRAG", "System Events");
			MouseEnter = AddSystemEvent("MOUSE ENTER", "System Events");
			MouseExit = AddSystemEvent("MOUSE EXIT", "System Events");
			MouseOver = AddSystemEvent("MOUSE OVER", "System Events");
			MouseUp = AddSystemEvent("MOUSE UP", "System Events");
			MouseUpAsButton = AddSystemEvent("MOUSE UP AS BUTTON", "System Events");
			CollisionEnter = AddSystemEvent("COLLISION ENTER", "System Events");
			CollisionExit = AddSystemEvent("COLLISION EXIT", "System Events");
			CollisionStay = AddSystemEvent("COLLISION STAY", "System Events");
			ControllerColliderHit = AddSystemEvent("CONTROLLER COLLIDER HIT", "System Events");
			TriggerEnter = AddSystemEvent("TRIGGER ENTER", "System Events");
			TriggerExit = AddSystemEvent("TRIGGER EXIT", "System Events");
			TriggerStay = AddSystemEvent("TRIGGER STAY", "System Events");
			CollisionEnter2D = AddSystemEvent("COLLISION ENTER 2D", "System Events");
			CollisionExit2D = AddSystemEvent("COLLISION EXIT 2D", "System Events");
			CollisionStay2D = AddSystemEvent("COLLISION STAY 2D", "System Events");
			TriggerEnter2D = AddSystemEvent("TRIGGER ENTER 2D", "System Events");
			TriggerExit2D = AddSystemEvent("TRIGGER EXIT 2D", "System Events");
			TriggerStay2D = AddSystemEvent("TRIGGER STAY 2D", "System Events");
			PlayerConnected = AddSystemEvent("PLAYER CONNECTED", "Network Events");
			ServerInitialized = AddSystemEvent("SERVER INITIALIZED", "Network Events");
			ConnectedToServer = AddSystemEvent("CONNECTED TO SERVER", "Network Events");
			PlayerDisconnected = AddSystemEvent("PLAYER DISCONNECTED", "Network Events");
			DisconnectedFromServer = AddSystemEvent("DISCONNECTED FROM SERVER", "Network Events");
			FailedToConnect = AddSystemEvent("FAILED TO CONNECT", "Network Events");
			FailedToConnectToMasterServer = AddSystemEvent("FAILED TO CONNECT TO MASTER SERVER", "Network Events");
			MasterServerEvent = AddSystemEvent("MASTER SERVER EVENT", "Network Events");
			NetworkInstantiate = AddSystemEvent("NETWORK INSTANTIATE", "Network Events");
			ApplicationFocus = AddSystemEvent("APPLICATION FOCUS", "System Events");
			ApplicationPause = AddSystemEvent("APPLICATION PAUSE", "System Events");
			ApplicationQuit = AddSystemEvent("APPLICATION QUIT", "System Events");
			ParticleCollision = AddSystemEvent("PARTICLE COLLISION", "System Events");
			JointBreak = AddSystemEvent("JOINT BREAK", "System Events");
			JointBreak2D = AddSystemEvent("JOINT BREAK 2D", "System Events");
		}

		private static FsmEvent AddSystemEvent(string eventName, string path = "")
		{
			FsmEvent fsmEvent = new FsmEvent(eventName);
			fsmEvent.IsSystemEvent = true;
			fsmEvent.Path = ((path == "") ? "" : (path + "/"));
			return fsmEvent;
		}

		private static void AddGlobalEvents()
		{
			for (int i = 0; i < globalEvents.Count; i++)
			{
				string text = globalEvents[i];
				FsmEvent fsmEvent = new FsmEvent(text);
				fsmEvent.isGlobal = true;
			}
		}

		public static void SanityCheckEventList()
		{
			foreach (FsmEvent @event in EventList)
			{
				if (IsEventGlobal(@event.name))
				{
					@event.isGlobal = true;
				}
				if (@event.isGlobal && !globalEvents.Contains(@event.name))
				{
					globalEvents.Add(@event.name);
				}
			}
			List<FsmEvent> list = new List<FsmEvent>();
			foreach (FsmEvent event2 in EventList)
			{
				if (!EventListContainsEvent(list, event2.Name))
				{
					list.Add(event2);
				}
			}
			EventList = list;
		}
	}
}
