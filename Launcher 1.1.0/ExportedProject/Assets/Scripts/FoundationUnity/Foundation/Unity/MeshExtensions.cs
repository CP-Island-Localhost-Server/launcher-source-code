using UnityEngine;

namespace Foundation.Unity
{
	public static class MeshExtensions
	{
		public static int GetHash(this Mesh mesh)
		{
			StructHash structHash = default(StructHash);
			structHash.Combine(mesh.bindposes);
			structHash.Combine(mesh.blendShapeCount);
			structHash.Combine(mesh.boneWeights);
			structHash.Combine(mesh.bounds);
			structHash.Combine(mesh.colors32);
			structHash.Combine(mesh.normals);
			structHash.Combine(mesh.subMeshCount);
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				structHash.Combine(mesh.GetIndices(i));
			}
			structHash.Combine(mesh.tangents);
			structHash.Combine(mesh.triangles);
			structHash.Combine(mesh.uv);
			structHash.Combine(mesh.uv2);
			structHash.Combine(mesh.uv3);
			structHash.Combine(mesh.uv4);
			structHash.Combine(mesh.vertexCount);
			structHash.Combine(mesh.vertices);
			return structHash;
		}
	}
}
