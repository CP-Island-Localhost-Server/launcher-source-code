using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	public class LauncherCardPrefabController : MonoBehaviour
	{
		public string Id;

		public Image MainImage;

		public GameObject[] TextBackgrounds;

		public Text Title;

		public Text Body;

		public float Duration = 5f;

		public string MainImageURL;

		public bool imageLoaded = false;

		private bool wwwInProgress = false;

		private WWW www;

		private void Update()
		{
			if (!imageLoaded && !string.IsNullOrEmpty(MainImageURL))
			{
				if (!wwwInProgress)
				{
					Debug.Log("get image MainImageURL=" + MainImageURL);
					www = new WWW(MainImageURL);
					wwwInProgress = true;
				}
				if (www.error == null && www.isDone)
				{
					MainImage.sprite = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0f, 0f));
					imageLoaded = true;
					wwwInProgress = false;
				}
			}
		}

		public void SetBackground(int theme)
		{
			int num = theme - 1;
			for (int i = 0; i < TextBackgrounds.Length; i++)
			{
				TextBackgrounds[i].SetActive(i == num);
			}
		}
	}
}
