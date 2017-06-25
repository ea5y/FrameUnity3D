using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : Singleton<ScenesManager> 
{
	public GameObject Managers;
	public GameObject Cameras;
	public GameObject Test;
	// Use this for initialization
	void Awake()
	{
		
		DontDestroyOnLoad(Managers);			
		DontDestroyOnLoad(Cameras);
		DontDestroyOnLoad(Test);
		//Application.LoadLevel("TestScene");
	}

	public void LoadingScene(string sceneName)
	{
		LoadingProgressData.nextScene = sceneName;
		LoadingProgressData.type = TOOLS.LoadingType.Scene;
		Application.LoadLevel("B_SceneLoading");
		Debug.Log("Test");
	}

	public void LoadingResource(string url)
	{
		LoadingProgressData.type = TOOLS.LoadingType.Resource;
		Application.LoadLevel("B_SceneLoading");
	}
}
