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
		LoadingSceneData.nextScene = sceneName;
		LoadingSceneData.type = LoadingType.Scene;
		Application.LoadLevel("B_SceneLoading");
		Debug.Log("Test");
	}

	public void ToLoadingScene(string nextScene, LoadingType type)
	{
		LoadingSceneData.nextScene = nextScene;
		LoadingSceneData.type = type;
		Application.LoadLevel("B_SceneLoading");
	}
}
