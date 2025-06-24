using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreLoad : MonoBehaviour {

	public float delay = 2f;
	void Start()
	{
		Invoke("LoadScene", delay);
	}

	void LoadScene()
	{
		Debug.Log(PlayerPrefs.GetString("LevelToLoad"));
		if(PlayerPrefs.GetString("LevelToLoad")  != "")
		{
			SceneManager.LoadScene(PlayerPrefs.GetString("LevelToLoad"));
		}
		else
		{
			SceneManager.LoadScene("1");
		}
	}
}
