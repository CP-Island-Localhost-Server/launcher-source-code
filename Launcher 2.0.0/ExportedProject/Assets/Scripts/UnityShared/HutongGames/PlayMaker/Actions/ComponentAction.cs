using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class ComponentAction<T> : FsmStateAction where T : Component
	{
		private GameObject cachedGameObject;

		private T component;

		protected Rigidbody rigidbody
		{
			get
			{
				return component as Rigidbody;
			}
		}

		protected Rigidbody2D rigidbody2d
		{
			get
			{
				return component as Rigidbody2D;
			}
		}

		protected Renderer renderer
		{
			get
			{
				return component as Renderer;
			}
		}

		protected Animation animation
		{
			get
			{
				return component as Animation;
			}
		}

		protected AudioSource audio
		{
			get
			{
				return component as AudioSource;
			}
		}

		protected Camera camera
		{
			get
			{
				return component as Camera;
			}
		}

		protected GUIText guiText
		{
			get
			{
				return component as GUIText;
			}
		}

		protected GUITexture guiTexture
		{
			get
			{
				return component as GUITexture;
			}
		}

		protected Light light
		{
			get
			{
				return component as Light;
			}
		}

		protected NetworkView networkView
		{
			get
			{
				return component as NetworkView;
			}
		}

		protected bool UpdateCache(GameObject go)
		{
			if (go == null)
			{
				return false;
			}
			if (component == null || cachedGameObject != go)
			{
				component = go.GetComponent<T>();
				cachedGameObject = go;
				if (component == null)
				{
					LogWarning("Missing component: " + typeof(T).FullName + " on: " + go.name);
				}
			}
			return component != null;
		}
	}
}
