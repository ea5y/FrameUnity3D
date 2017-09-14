﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Easy.FrameUnity.Manager;
using Easy.FrameUnity.Net;

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
        AsyncOperation op;
        switch(name)
        {
            case ScenesName.A_SceneEnter:
                break;
            case ScenesName.B_SceneLoading:
                break;
            case ScenesName.C_SceneLogin:
                op = SceneManager.LoadSceneAsync("C_SceneLogin");
                break;
            case ScenesName.D_SceneGameInit:
                break;
            case ScenesName.E_SceneGame_1:
                op = SceneManager.LoadSceneAsync("E_SceneGame_1");
                StartCoroutine(Loading(op, 
                            () => 
                            { 
                                EasyTouchPlugin.Inst.Enable(true);
                                Net.SpwanPlayer();
                            }));
                break;
            case ScenesName.F_SceneGame_2:
                break;
        }
    }

    private IEnumerator Loading(AsyncOperation op, Action action)
    {
        while(!op.isDone)
        {
            yield return null;
        }
        action.Invoke();
    }
}
