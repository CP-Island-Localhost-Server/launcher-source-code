using System;
using System.Collections.Generic;
using SwrveUnity;
using SwrveUnityMiniJSON;
using UnityEngine;

public class SwrveComponent : MonoBehaviour
{
	public SwrveSDK SDK;

	public int AppId = 0;

	public string ApiKey = "your_api_key_here";

	public SwrveConfig Config;

	public bool FlushEventsOnApplicationQuit = true;

	public bool InitialiseOnStart = true;

	protected static SwrveComponent instance;

	public static SwrveComponent Instance
	{
		get
		{
			if (!instance)
			{
				SwrveComponent[] array = UnityEngine.Object.FindObjectsOfType(typeof(SwrveComponent)) as SwrveComponent[];
				if (array != null && array.Length > 0)
				{
					instance = array[0];
				}
				else
				{
					SwrveLog.LogError("There needs to be one active SwrveComponent script on a GameObject in your scene.");
				}
			}
			return instance;
		}
	}

	public SwrveComponent()
	{
		Config = new SwrveConfig();
		SDK = new SwrveSDK();
	}

	public void Init(int appId, string apiKey)
	{
		SDK.Init(this, appId, apiKey, Config);
	}

	public void Start()
	{
		base.useGUILayout = false;
		if (InitialiseOnStart)
		{
			Init(AppId, ApiKey);
		}
	}

	public void Update()
	{
		if (SDK != null && SDK.Initialised)
		{
			SDK.Update();
		}
	}

	public void OnDestroy()
	{
		if (SDK.Initialised)
		{
			SDK.OnSwrveDestroy();
		}
		StopAllCoroutines();
	}

	public void OnApplicationQuit()
	{
		if (SDK.Initialised && FlushEventsOnApplicationQuit)
		{
			SDK.OnSwrveDestroy();
		}
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		if (SDK != null && SDK.Initialised && Config != null && Config.AutomaticSessionManagement)
		{
			if (pauseStatus)
			{
				SDK.OnSwrvePause();
			}
			else
			{
				SDK.OnSwrveResume();
			}
		}
	}

	public void SetLocationSegmentVersion(string locationSegmentVersion)
	{
		try
		{
			SDK.SetLocationSegmentVersion(int.Parse(locationSegmentVersion));
		}
		catch (Exception ex)
		{
			SwrveLog.LogError(ex.ToString(), "location");
		}
	}

	public void UserUpdate(string userUpdate)
	{
		try
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(userUpdate);
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			Dictionary<string, object>.Enumerator enumerator = dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				dictionary2[enumerator.Current.Key] = string.Format("{0}", enumerator.Current.Value);
			}
			SDK.UserUpdate(dictionary2);
		}
		catch (Exception ex)
		{
			SwrveLog.LogError(ex.ToString(), "userUpdate");
		}
	}
}
