using System.IO;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	[CreateAssetMenu(menuName = "Scene/Prerequisite Content Paths", fileName = "ScenePrereq_[SceneName]")]
	public class ScenePrereqContentPaths : ScriptableObject
	{
		[Scene]
		[Tooltip("The Scene object to reference")]
		public string Scene;

		[Header("Path to the Required Content")]
		[Tooltip("The path to the folder content")]
		[FolderPath(true)]
		public ContentPath[] ContentPaths;

		[HideInInspector]
		public string SceneName
		{
			get
			{
				return Path.GetFileNameWithoutExtension(Scene);
			}
		}
	}
}
