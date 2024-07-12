using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public static class ActionHelpers
	{
		private static Texture2D whiteTexture;

		public static RaycastHit mousePickInfo;

		private static float mousePickRaycastTime;

		private static float mousePickDistanceUsed;

		private static int mousePickLayerMaskUsed;

		public static Texture2D WhiteTexture
		{
			get
			{
				if (whiteTexture == null)
				{
					whiteTexture = new Texture2D(1, 1);
					whiteTexture.SetPixel(0, 0, Color.white);
					whiteTexture.Apply();
				}
				return whiteTexture;
			}
		}

		public static bool IsVisible(GameObject go)
		{
			if (go == null)
			{
				return false;
			}
			Renderer component = go.GetComponent<Renderer>();
			if (component != null)
			{
				return component.isVisible;
			}
			return false;
		}

		[Obsolete("Use LogError instead.")]
		public static void RuntimeError(FsmStateAction action, string error)
		{
			action.LogError(string.Concat(action, " : ", error));
		}

		public static PlayMakerFSM GetGameObjectFsm(GameObject go, string fsmName)
		{
			if (!string.IsNullOrEmpty(fsmName))
			{
				PlayMakerFSM[] components = go.GetComponents<PlayMakerFSM>();
				PlayMakerFSM[] array = components;
				foreach (PlayMakerFSM playMakerFSM in array)
				{
					if (playMakerFSM.FsmName == fsmName)
					{
						return playMakerFSM;
					}
				}
				Debug.LogWarning("Could not find FSM: " + fsmName);
			}
			return go.GetComponent<PlayMakerFSM>();
		}

		public static int GetRandomWeightedIndex(FsmFloat[] weights)
		{
			float num = 0f;
			foreach (FsmFloat fsmFloat in weights)
			{
				num += fsmFloat.Value;
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			for (int j = 0; j < weights.Length; j++)
			{
				if (num2 < weights[j].Value)
				{
					return j;
				}
				num2 -= weights[j].Value;
			}
			return -1;
		}

		public static bool HasAnimationFinished(AnimationState anim, float prevTime, float currentTime)
		{
			if (anim.wrapMode == WrapMode.Loop || anim.wrapMode == WrapMode.PingPong)
			{
				return false;
			}
			if ((anim.wrapMode == WrapMode.Default || anim.wrapMode == WrapMode.Once) && prevTime > 0f && currentTime.Equals(0f))
			{
				return true;
			}
			if (prevTime < anim.length)
			{
				return currentTime >= anim.length;
			}
			return false;
		}

		public static Vector3 GetPosition(FsmGameObject fsmGameObject, FsmVector3 fsmVector3)
		{
			if (fsmGameObject.Value != null)
			{
				return (!fsmVector3.IsNone) ? fsmGameObject.Value.transform.TransformPoint(fsmVector3.Value) : fsmGameObject.Value.transform.position;
			}
			return fsmVector3.Value;
		}

		public static bool IsMouseOver(GameObject gameObject, float distance, int layerMask)
		{
			if (gameObject == null)
			{
				return false;
			}
			return gameObject == MouseOver(distance, layerMask);
		}

		public static RaycastHit MousePick(float distance, int layerMask)
		{
			if (!mousePickRaycastTime.Equals(Time.frameCount) || mousePickDistanceUsed < distance || mousePickLayerMaskUsed != layerMask)
			{
				DoMousePick(distance, layerMask);
			}
			return mousePickInfo;
		}

		public static GameObject MouseOver(float distance, int layerMask)
		{
			if (!mousePickRaycastTime.Equals(Time.frameCount) || mousePickDistanceUsed < distance || mousePickLayerMaskUsed != layerMask)
			{
				DoMousePick(distance, layerMask);
			}
			if (mousePickInfo.collider != null && mousePickInfo.distance < distance)
			{
				return mousePickInfo.collider.gameObject;
			}
			return null;
		}

		private static void DoMousePick(float distance, int layerMask)
		{
			if (!(Camera.main == null))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Physics.Raycast(ray, out mousePickInfo, distance, layerMask);
				mousePickLayerMaskUsed = layerMask;
				mousePickDistanceUsed = distance;
				mousePickRaycastTime = Time.frameCount;
			}
		}

		public static int LayerArrayToLayerMask(FsmInt[] layers, bool invert)
		{
			int num = 0;
			foreach (FsmInt fsmInt in layers)
			{
				num |= 1 << fsmInt.Value;
			}
			if (invert)
			{
				num = ~num;
			}
			if (num != 0)
			{
				return num;
			}
			return -5;
		}

		public static bool IsLoopingWrapMode(WrapMode wrapMode)
		{
			if (wrapMode != WrapMode.Loop)
			{
				return wrapMode == WrapMode.PingPong;
			}
			return true;
		}

		public static string CheckRayDistance(float rayDistance)
		{
			if (!(rayDistance <= 0f))
			{
				return "";
			}
			return "Ray Distance should be greater than zero!\n";
		}

		public static string CheckForValidEvent(FsmState state, string eventName)
		{
			if (state == null)
			{
				return "Invalid State!";
			}
			if (string.IsNullOrEmpty(eventName))
			{
				return "";
			}
			FsmTransition[] globalTransitions = state.Fsm.GlobalTransitions;
			foreach (FsmTransition fsmTransition in globalTransitions)
			{
				if (fsmTransition.EventName == eventName)
				{
					return "";
				}
			}
			FsmTransition[] transitions = state.Transitions;
			foreach (FsmTransition fsmTransition2 in transitions)
			{
				if (fsmTransition2.EventName == eventName)
				{
					return "";
				}
			}
			return "Fsm will not respond to Event: " + eventName;
		}

		public static string CheckPhysicsSetup(FsmOwnerDefault ownerDefault)
		{
			if (ownerDefault == null)
			{
				return "";
			}
			if (ownerDefault.OwnerOption != 0)
			{
				return CheckPhysicsSetup(ownerDefault.GameObject.Value);
			}
			return CheckOwnerPhysicsSetup(ownerDefault.GameObject.Value);
		}

		public static string CheckOwnerPhysicsSetup(GameObject gameObject)
		{
			string text = string.Empty;
			if (gameObject != null && gameObject.GetComponent<Collider>() == null && gameObject.GetComponent<Rigidbody>() == null)
			{
				text += "Owner requires a RigidBody or Collider component!\n";
			}
			return text;
		}

		public static string CheckOwnerPhysics2dSetup(GameObject gameObject)
		{
			string text = string.Empty;
			if (gameObject != null && gameObject.GetComponent<Collider2D>() == null && gameObject.GetComponent<Rigidbody2D>() == null)
			{
				text += "Owner requires a RigidBody2D or Collider2D component!\n";
			}
			return text;
		}

		public static string CheckPhysicsSetup(GameObject gameObject)
		{
			string text = string.Empty;
			if (gameObject != null && gameObject.GetComponent<Collider>() == null && gameObject.GetComponent<Rigidbody>() == null)
			{
				text += "GameObject requires RigidBody/Collider!\n";
			}
			return text;
		}

		public static void DebugLog(Fsm fsm, LogLevel logLevel, string text, bool sendToUnityLog = false)
		{
			if (FsmLog.LoggingEnabled && fsm != null)
			{
				switch (logLevel)
				{
				case LogLevel.Info:
					fsm.MyLog.LogAction(FsmLogType.Info, text, sendToUnityLog);
					break;
				case LogLevel.Warning:
					fsm.MyLog.LogAction(FsmLogType.Warning, text, sendToUnityLog);
					break;
				case LogLevel.Error:
					fsm.MyLog.LogAction(FsmLogType.Error, text, sendToUnityLog);
					break;
				}
			}
		}

		public static void LogError(string text)
		{
			DebugLog(FsmExecutionStack.ExecutingFsm, LogLevel.Error, text, true);
		}

		public static void LogWarning(string text)
		{
			DebugLog(FsmExecutionStack.ExecutingFsm, LogLevel.Warning, text, true);
		}

		public static string GetValueLabel(INamedVariable variable)
		{
			if (variable == null)
			{
				return "[null]";
			}
			if (variable.IsNone)
			{
				return "[none]";
			}
			if (variable.UseVariable)
			{
				return variable.Name;
			}
			object rawValue = variable.RawValue;
			if (object.ReferenceEquals(rawValue, null))
			{
				return "null";
			}
			if (rawValue is string)
			{
				return string.Concat("\"", rawValue, "\"");
			}
			if (rawValue is Array)
			{
				return "Array";
			}
			return variable.RawValue.ToString();
		}

		public static string GetValueLabel(Fsm fsm, FsmOwnerDefault ownerDefault)
		{
			if (ownerDefault == null)
			{
				return "[null]";
			}
			if (ownerDefault.OwnerOption == OwnerDefaultOption.UseOwner)
			{
				return "Owner";
			}
			return GetValueLabel(ownerDefault.GameObject);
		}

		public static void AddAnimationClip(GameObject go, AnimationClip animClip)
		{
			if (!(animClip == null))
			{
				Animation component = go.GetComponent<Animation>();
				if (component != null)
				{
					component.AddClip(animClip, animClip.name);
				}
			}
		}
	}
}
