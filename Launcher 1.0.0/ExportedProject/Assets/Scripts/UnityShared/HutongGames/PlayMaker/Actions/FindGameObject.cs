using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Finds a Game Object by Name and/or Tag.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class FindGameObject : FsmStateAction
	{
		[Tooltip("The name of the GameObject to find. You can leave this empty if you specify a Tag.")]
		public FsmString objectName;

		[UIHint(UIHint.Tag)]
		[Tooltip("Find a GameObject with this tag. If Object Name is specified then both name and Tag must match.")]
		public FsmString withTag;

		[Tooltip("Store the result in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject store;

		public override void Reset()
		{
			objectName = "";
			withTag = "Untagged";
			store = null;
		}

		public override void OnEnter()
		{
			Find();
			Finish();
		}

		private void Find()
		{
			if (withTag.Value != "Untagged")
			{
				if (!string.IsNullOrEmpty(objectName.Value))
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag(withTag.Value);
					GameObject[] array2 = array;
					foreach (GameObject gameObject in array2)
					{
						if (gameObject.name == objectName.Value)
						{
							store.Value = gameObject;
							return;
						}
					}
					store.Value = null;
				}
				else
				{
					store.Value = GameObject.FindGameObjectWithTag(withTag.Value);
				}
			}
			else
			{
				store.Value = GameObject.Find(objectName.Value);
			}
		}

		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(objectName.Value) && string.IsNullOrEmpty(withTag.Value))
			{
				return "Specify Name, Tag, or both.";
			}
			return null;
		}
	}
}
