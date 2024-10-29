using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;


public class FadeManager : Singleton<FadeManager>
{
	private float fadeAlpha = 0;

	[NonSerialized] public bool isFading = false;

	public Color fadeColor = Color.black;

	public void Awake ()
	{
		if (gameObject.transform.parent != null)
		{
			gameObject.transform.parent = null;
		}

		if (this != Instance)
        {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);
	}

	public void OnGUI ()
	{
		if (isFading == true)
		{
			fadeColor.a = fadeAlpha;
			GUI.color = fadeColor;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}
	}

	public void LoadScene (string scene, float interval)
	{
		StartCoroutine (TransScene (scene, interval));
	}

	public void LoadSceneIndex(int sceneIndex, float interval)
	{
		StartCoroutine(TransSceneIndex(sceneIndex, interval));
	}


	private IEnumerator TransScene (string scene, float interval)
	{
		StartCoroutine(SoundManager.Instance.FadeOut(interval*0.9f));

		isFading = true;
		float time = 0;
		while (time <= interval)
        {
			fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);
            time += Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene (scene);

		StartCoroutine(SoundManager.Instance.FadeIn(interval * 0.9f));

		time = 0;
		while (time <= interval)
        {
			fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
            time += Time.deltaTime;
			yield return null;
		}

		isFading = false;
	}

	private IEnumerator TransSceneIndex(int sceneIndex, float interval)
	{
		StartCoroutine(SoundManager.Instance.FadeOut(interval * 0.9f));

		isFading = true;
		float time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene(sceneIndex);

		StartCoroutine(SoundManager.Instance.FadeIn(interval * 0.9f));

		time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}

		isFading = false;
	}
}
