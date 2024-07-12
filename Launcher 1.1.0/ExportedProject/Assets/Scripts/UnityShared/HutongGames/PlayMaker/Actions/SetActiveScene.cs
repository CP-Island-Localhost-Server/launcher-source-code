using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the scene to be active.")]
	[ActionCategory(ActionCategory.Scene)]
	public class SetActiveScene : FsmStateAction
	{
		public enum SceneReferenceOptions
		{
			SceneAtBuildIndex = 0,
			SceneAtIndex = 1,
			SceneByName = 2,
			SceneByPath = 3,
			SceneByGameObject = 4
		}

		[Tooltip("The reference options of the Scene")]
		public SceneReferenceOptions sceneReference;

		[Tooltip("The name of the scene to activate. The given sceneName can either be the last part of the path, without .unity extension or the full path still without the .unity extension")]
		public FsmString sceneByName;

		[Tooltip("The build index of the scene to activate.")]
		public FsmInt sceneAtBuildIndex;

		[Tooltip("The index of the scene to activae.")]
		public FsmInt sceneAtIndex;

		[Tooltip("The scene Path.")]
		public FsmString sceneByPath;

		[Tooltip("The GameObject scene to activate")]
		public FsmOwnerDefault sceneByGameObject;

		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("True if set active succedded")]
		public FsmBool success;

		[Tooltip("Event sent if setActive succedded ")]
		public FsmEvent successEvent;

		[Tooltip("True if SceneReference resolves to a scene")]
		[UIHint(UIHint.Variable)]
		public FsmBool sceneFound;

		[UIHint(UIHint.Variable)]
		[Tooltip("Event sent if scene not activated yet")]
		public FsmEvent sceneNotActivatedEvent;

		[Tooltip("Event sent if SceneReference do not resolve to a scene")]
		public FsmEvent sceneNotFoundEvent;

		private Scene _scene;

		private bool _sceneFound;

		private bool _success;

		public override void Reset()
		{
			sceneReference = SceneReferenceOptions.SceneAtIndex;
			sceneByName = null;
			sceneAtBuildIndex = null;
			sceneAtIndex = null;
			sceneByPath = null;
			sceneByGameObject = null;
			success = null;
			successEvent = null;
			sceneFound = null;
			sceneNotActivatedEvent = null;
			sceneNotFoundEvent = null;
		}

		public override void OnEnter()
		{
			DoSetActivate();
			if (!success.IsNone)
			{
				success.Value = _success;
			}
			if (!sceneFound.IsNone)
			{
				sceneFound.Value = _sceneFound;
			}
			if (_success)
			{
				base.Fsm.Event(successEvent);
			}
		}

		private void DoSetActivate()
		{
			try
			{
				switch (sceneReference)
				{
				case SceneReferenceOptions.SceneAtIndex:
					_scene = SceneManager.GetSceneAt(sceneAtIndex.Value);
					break;
				case SceneReferenceOptions.SceneByName:
					_scene = SceneManager.GetSceneByName(sceneByName.Value);
					break;
				case SceneReferenceOptions.SceneByPath:
					_scene = SceneManager.GetSceneByPath(sceneByPath.Value);
					break;
				case SceneReferenceOptions.SceneByGameObject:
				{
					GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(sceneByGameObject);
					if (ownerDefaultTarget == null)
					{
						throw new Exception("Null GameObject");
					}
					_scene = ownerDefaultTarget.scene;
					break;
				}
				}
			}
			catch (Exception ex)
			{
				LogError(ex.Message);
				_sceneFound = false;
				base.Fsm.Event(sceneNotFoundEvent);
				return;
			}
			if (_scene == default(Scene))
			{
				_sceneFound = false;
				base.Fsm.Event(sceneNotFoundEvent);
			}
			else
			{
				_success = SceneManager.SetActiveScene(_scene);
				_sceneFound = true;
			}
		}
	}
}
