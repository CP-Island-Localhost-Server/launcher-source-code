using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmOwnerDefault
	{
		[SerializeField]
		private OwnerDefaultOption ownerOption;

		[SerializeField]
		private FsmGameObject gameObject;

		public OwnerDefaultOption OwnerOption
		{
			get
			{
				return ownerOption;
			}
			set
			{
				ownerOption = value;
			}
		}

		public FsmGameObject GameObject
		{
			get
			{
				return gameObject;
			}
			set
			{
				gameObject = value;
			}
		}

		public FsmOwnerDefault()
		{
			ownerOption = OwnerDefaultOption.UseOwner;
			gameObject = new FsmGameObject(string.Empty);
		}

		public FsmOwnerDefault(FsmOwnerDefault source)
		{
			if (source != null)
			{
				ownerOption = source.ownerOption;
				gameObject = new FsmGameObject(source.GameObject);
			}
		}
	}
}
