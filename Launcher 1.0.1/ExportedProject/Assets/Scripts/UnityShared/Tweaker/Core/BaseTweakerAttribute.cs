using System;
using UnityEngine.Scripting;

namespace Tweaker.Core
{
	public abstract class BaseTweakerAttribute : PreserveAttribute, ITweakerAttribute
	{
		public string Description = "";

		public string Name { get; private set; }

		public Guid Guid { get; private set; }

		protected BaseTweakerAttribute(string name)
		{
			Name = name;
			Guid = Guid.NewGuid();
		}
	}
}
