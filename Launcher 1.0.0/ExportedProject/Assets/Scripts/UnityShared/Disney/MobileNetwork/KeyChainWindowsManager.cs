using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using UnityEngine;

namespace Disney.MobileNetwork
{
	public class KeyChainWindowsManager : KeyChainManager
	{
		private const string APP_DATA_KEY = "cp.AppData";

		[DllImport("KeyChainWindows", CharSet = CharSet.Ansi)]
		private static extern void _cryptProtectData(string dataIn, ref int dataOutSize, out IntPtr dataOut);

		[DllImport("KeyChainWindows", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private static extern string _cryptUnprotectData(byte[] dataIn, int dataInLength);

		protected override void Init()
		{
			appData = getAppData();
		}

		public override void PutString(string key, string value)
		{
			appData[key] = value;
			setAppData(appData);
		}

		public override string GetString(string key)
		{
			appData = getAppData();
			string value;
			appData.TryGetValue(key, out value);
			return value;
		}

		public override void RemoveString(string key)
		{
			if (appData.ContainsKey(key))
			{
				appData.Remove(key);
				setAppData(appData);
			}
		}

		private void setAppData(Dictionary<string, string> data)
		{
			if (data == null)
			{
				PlayerPrefs.DeleteKey("cp.AppData");
			}
			string dataIn = JsonMapper.ToJson(data);
			int dataOutSize = 0;
			IntPtr dataOut;
			_cryptProtectData(dataIn, ref dataOutSize, out dataOut);
			byte[] array = new byte[dataOutSize];
			Marshal.Copy(dataOut, array, 0, dataOutSize);
			Marshal.FreeCoTaskMem(dataOut);
			string value = Convert.ToBase64String(array);
			PlayerPrefs.SetString("cp.AppData", value);
		}

		private Dictionary<string, string> getAppData()
		{
			string @string = PlayerPrefs.GetString("cp.AppData", null);
			if (!string.IsNullOrEmpty(@string))
			{
				byte[] array = Convert.FromBase64String(@string);
				string json = _cryptUnprotectData(array, array.Length);
				return JsonMapper.ToObject<Dictionary<string, string>>(json);
			}
			return new Dictionary<string, string>();
		}
	}
}
