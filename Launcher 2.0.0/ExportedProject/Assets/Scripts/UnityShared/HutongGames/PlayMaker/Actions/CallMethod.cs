using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Call a method in a behaviour.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class CallMethod : FsmStateAction
	{
		[Tooltip("Store the component in an Object variable.\nNOTE: Set theObject variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
		[ObjectType(typeof(MonoBehaviour))]
		public FsmObject behaviour;

		[Tooltip("Name of the method to call on the component")]
		public FsmString methodName;

		[Tooltip("Method paramters. NOTE: these must match the method's signature!")]
		public FsmVar[] parameters;

		[UIHint(UIHint.Variable)]
		[ActionSection("Store Result")]
		[Tooltip("Store the result of the method call.")]
		public FsmVar storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[Tooltip("Use the old manual editor UI.")]
		public bool manualUI;

		private FsmObject cachedBehaviour;

		private FsmString cachedMethodName;

		private Type cachedType;

		private MethodInfo cachedMethodInfo;

		private ParameterInfo[] cachedParameterInfo;

		private object[] parametersArray;

		private string errorString;

		public override void Reset()
		{
			behaviour = null;
			methodName = null;
			parameters = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			parametersArray = new object[parameters.Length];
			DoMethodCall();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMethodCall();
		}

		private void DoMethodCall()
		{
			if (behaviour.Value == null)
			{
				Finish();
				return;
			}
			if (NeedToUpdateCache() && !DoCache())
			{
				Debug.LogError(errorString);
				Finish();
				return;
			}
			object value;
			if (cachedParameterInfo.Length == 0)
			{
				value = cachedMethodInfo.Invoke(cachedBehaviour.Value, null);
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					FsmVar fsmVar = parameters[i];
					fsmVar.UpdateValue();
					if (fsmVar.Type == VariableType.Array)
					{
						fsmVar.UpdateValue();
						object[] array = fsmVar.GetValue() as object[];
						Type elementType = cachedParameterInfo[i].ParameterType.GetElementType();
						Array array2 = Array.CreateInstance(elementType, array.Length);
						for (int j = 0; j < array.Length; j++)
						{
							array2.SetValue(array[j], j);
						}
						parametersArray[i] = array2;
					}
					else
					{
						fsmVar.UpdateValue();
						parametersArray[i] = fsmVar.GetValue();
					}
				}
				value = cachedMethodInfo.Invoke(cachedBehaviour.Value, parametersArray);
			}
			if (storeResult != null && !storeResult.IsNone && storeResult.Type != VariableType.Unknown)
			{
				storeResult.SetValue(value);
			}
		}

		private bool NeedToUpdateCache()
		{
			return cachedBehaviour == null || cachedMethodName == null || cachedBehaviour.Value != behaviour.Value || cachedBehaviour.Name != behaviour.Name || cachedMethodName.Value != methodName.Value || cachedMethodName.Name != methodName.Name;
		}

		private void ClearCache()
		{
			cachedBehaviour = null;
			cachedMethodName = null;
			cachedType = null;
			cachedMethodInfo = null;
			cachedParameterInfo = null;
		}

		private bool DoCache()
		{
			ClearCache();
			errorString = string.Empty;
			cachedBehaviour = new FsmObject(behaviour);
			cachedMethodName = new FsmString(methodName);
			if (cachedBehaviour.Value == null)
			{
				if (!behaviour.UsesVariable || Application.isPlaying)
				{
					errorString += "Behaviour is invalid!\n";
				}
				Finish();
				return false;
			}
			cachedType = behaviour.Value.GetType();
			List<Type> list = new List<Type>(parameters.Length);
			FsmVar[] array = parameters;
			foreach (FsmVar fsmVar in array)
			{
				list.Add(fsmVar.RealType);
			}
			cachedMethodInfo = cachedType.GetMethod(methodName.Value, list.ToArray());
			if (cachedMethodInfo == null)
			{
				errorString = errorString + "Invalid Method Name or Parameters: " + methodName.Value + "\n";
				Finish();
				return false;
			}
			cachedParameterInfo = cachedMethodInfo.GetParameters();
			return true;
		}

		public override string ErrorCheck()
		{
			if (Application.isPlaying)
			{
				return errorString;
			}
			if (!DoCache())
			{
				return errorString;
			}
			if (parameters.Length != cachedParameterInfo.Length)
			{
				return "Parameter count does not match method.\nMethod has " + cachedParameterInfo.Length + " parameters.\nYou specified " + parameters.Length + " paramaters.";
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				FsmVar fsmVar = parameters[i];
				Type realType = fsmVar.RealType;
				Type parameterType = cachedParameterInfo[i].ParameterType;
				if (!object.ReferenceEquals(realType, parameterType))
				{
					return string.Concat("Parameters do not match method signature.\nParameter ", i + 1, " (", realType, ") should be of type: ", parameterType);
				}
			}
			if (object.ReferenceEquals(cachedMethodInfo.ReturnType, typeof(void)))
			{
				if (!string.IsNullOrEmpty(storeResult.variableName))
				{
					return "Method does not have return.\nSpecify 'none' in Store Result.";
				}
			}
			else if (!object.ReferenceEquals(cachedMethodInfo.ReturnType, storeResult.RealType))
			{
				return "Store Result is of the wrong type.\nIt should be of type: " + cachedMethodInfo.ReturnType;
			}
			return string.Empty;
		}
	}
}
