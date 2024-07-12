using UnityEngine;

namespace HutongGames.PlayMaker
{
	public class FsmEventData
	{
		public Fsm SentByFsm;

		public FsmState SentByState;

		public FsmStateAction SentByAction;

		public bool BoolData;

		public int IntData;

		public float FloatData;

		public Vector2 Vector2Data;

		public Vector3 Vector3Data;

		public string StringData;

		public Quaternion QuaternionData;

		public Rect RectData;

		public Color ColorData;

		public Object ObjectData;

		public GameObject GameObjectData;

		public Material MaterialData;

		public Texture TextureData;

		public NetworkPlayer Player;

		public NetworkDisconnection DisconnectionInfo;

		public NetworkConnectionError ConnectionError;

		public NetworkMessageInfo NetworkMessageInfo;

		public MasterServerEvent MasterServerEvent;

		public FsmEventData()
		{
		}

		public FsmEventData(FsmEventData source)
		{
			SentByFsm = source.SentByFsm;
			SentByState = source.SentByState;
			SentByAction = source.SentByAction;
			BoolData = source.BoolData;
			IntData = source.IntData;
			FloatData = source.FloatData;
			Vector2Data = source.Vector2Data;
			Vector3Data = source.Vector3Data;
			StringData = source.StringData;
			QuaternionData = source.QuaternionData;
			RectData = source.RectData;
			ColorData = source.ColorData;
			ObjectData = source.ObjectData;
			GameObjectData = source.GameObjectData;
			MaterialData = source.MaterialData;
			TextureData = source.TextureData;
			Player = source.Player;
			DisconnectionInfo = source.DisconnectionInfo;
			ConnectionError = source.ConnectionError;
			NetworkMessageInfo = source.NetworkMessageInfo;
			MasterServerEvent = source.MasterServerEvent;
		}

		public void DebugLog()
		{
			Debug.Log("Sent By FSM: " + ((SentByFsm != null) ? SentByFsm.Name : "None"));
			Debug.Log("Sent By State: " + ((SentByState != null) ? SentByState.Name : "None"));
			Debug.Log("Sent By Action: " + ((SentByAction != null) ? SentByAction.GetType().Name : "None"));
		}
	}
}
