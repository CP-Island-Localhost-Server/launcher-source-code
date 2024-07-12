using System;

namespace Tweaker.UI
{
	public abstract class BaseNode : TreeNode<BaseNode>
	{
		public enum NodeType
		{
			Unknown = 0,
			Group = 1,
			Invokable = 2,
			Tweakable = 3,
			Watchable = 4
		}

		public virtual NodeType Type
		{
			get
			{
				return NodeType.Unknown;
			}
		}

		public string FullName { get; set; }

		public BaseNode(string fullName)
		{
			FullName = fullName;
			if (Type == NodeType.Unknown)
			{
				throw new Exception("NodeType for '" + fullName + "' must be overriden in parent and must not be Unknown.");
			}
		}
	}
}
