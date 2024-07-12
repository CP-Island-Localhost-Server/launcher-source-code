using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Set the Tag on all children of a GameObject. Optionally filter by component.")]
	public class SetTagsOnChildren : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject Parent")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Tag)]
		[Tooltip("Set Tag To...")]
		[RequiredField]
		public FsmString tag;

		[Tooltip("Only set the Tag on children with this component.")]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString filterByComponent;

		private Type componentFilter;

		public override void Reset()
		{
			gameObject = null;
			tag = null;
			filterByComponent = null;
		}

		public override void OnEnter()
		{
			SetTag(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void SetTag(GameObject parent)
		{
			if (parent == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(filterByComponent.Value))
			{
				foreach (Transform item in parent.transform)
				{
					item.gameObject.tag = tag.Value;
				}
			}
			else
			{
				UpdateComponentFilter();
				if (componentFilter != null)
				{
					Component[] componentsInChildren = parent.GetComponentsInChildren(componentFilter);
					Component[] array = componentsInChildren;
					foreach (Component component in array)
					{
						component.gameObject.tag = tag.Value;
					}
				}
			}
			Finish();
		}

		private void UpdateComponentFilter()
		{
			componentFilter = ReflectionUtils.GetGlobalType(filterByComponent.Value);
			if (componentFilter == null)
			{
				componentFilter = ReflectionUtils.GetGlobalType("UnityEngine." + filterByComponent.Value);
			}
			if (componentFilter == null)
			{
				Debug.LogWarning("Couldn't get type: " + filterByComponent.Value);
			}
		}
	}
}
