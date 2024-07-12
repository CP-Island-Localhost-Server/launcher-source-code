using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the position of a vertex in a GameObject's mesh. Hint: Use GetVertexCount to get the number of vertices in a mesh.")]
	[ActionCategory("Mesh")]
	public class GetVertexPosition : FsmStateAction
	{
		[Tooltip("The GameObject to check.")]
		[CheckForComponent(typeof(MeshFilter))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The index of the vertex.")]
		[RequiredField]
		public FsmInt vertexIndex;

		[Tooltip("Coordinate system to use.")]
		public Space space;

		[Tooltip("Store the vertex position in a variable.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storePosition;

		[Tooltip("Repeat every frame. Useful if the mesh is animated.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			space = Space.World;
			storePosition = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVertexPosition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetVertexPosition();
		}

		private void DoGetVertexPosition()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget != null))
			{
				return;
			}
			MeshFilter component = ownerDefaultTarget.GetComponent<MeshFilter>();
			if (component == null)
			{
				LogError("Missing MeshFilter!");
				return;
			}
			switch (space)
			{
			case Space.World:
			{
				Vector3 position = component.mesh.vertices[vertexIndex.Value];
				storePosition.Value = ownerDefaultTarget.transform.TransformPoint(position);
				break;
			}
			case Space.Self:
				storePosition.Value = component.mesh.vertices[vertexIndex.Value];
				break;
			}
		}
	}
}
