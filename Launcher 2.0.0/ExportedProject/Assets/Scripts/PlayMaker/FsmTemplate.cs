using System;
using HutongGames.PlayMaker;
using UnityEngine;

[Serializable]
public class FsmTemplate : ScriptableObject
{
	[SerializeField]
	private string category;

	public Fsm fsm;

	public string Category
	{
		get
		{
			return category;
		}
		set
		{
			category = value;
		}
	}

	public void OnEnable()
	{
		if (fsm != null)
		{
			fsm.UsedInTemplate = this;
		}
	}
}
