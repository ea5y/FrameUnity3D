using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum ScenesName
{
    A_SceneEnter,
    B_SceneLoading,
    C_SceneLogin,
    D_SceneGameInit,
    E_SceneGame_1,
    F_SceneGame_2
}

public class ScenesManager : Singleton<ScenesManager> 
{
    private void Awake()
    {
        base.GetInstance();
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

    public void EnterScene(ScenesName name)
    {
        switch(name)
        {
            case ScenesName.A_SceneEnter:
                break;
            case ScenesName.B_SceneLoading:
                break;
            case ScenesName.C_SceneLogin:
                Application.LoadLevel("C_SceneLogin");
                break;
            case ScenesName.D_SceneGameInit:
                break;
            case ScenesName.E_SceneGame_1:
                Application.LoadLevel("E_SceneGame_1");
                //SceneManager.LoadScene("E_SceneGame_1");
                break;
            case ScenesName.F_SceneGame_2:
                break;
        }
    }
}
