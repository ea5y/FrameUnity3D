using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Easy.FrameUnity.Manager;
using Easy.FrameUnity.Net;
using Easy.FrameUnity.EsAssetBundle;
using Easy.FrameUnity.ScriptableObj;

public enum SceneName
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

    public void EnterLoadingScene(SceneName nextScene, LoadingType type = LoadingType.Scene)
    {
        LoadingSceneData.NextScene = nextScene;
        LoadingSceneData.type = type;
        SceneManager.LoadSceneAsync("B_SceneLoading");
    }

    public void RealLoadScene(SceneName name, PanelLoading ui)
    {
        ui.SetUI(LoadingType.Scene);
        AsyncOperation op = null;
        Action callback = () => { };
        switch(name)
        {
            case SceneName.A_SceneEnter:
                break;
            case SceneName.B_SceneLoading:
                op = SceneManager.LoadSceneAsync("B_SceneLoading");
                break;
            case SceneName.C_SceneLogin:
                op = SceneManager.LoadSceneAsync("C_SceneLogin");
                break;
            case SceneName.D_SceneGameInit:
                break;
            case SceneName.E_SceneGame_1:
                op = SceneManager.LoadSceneAsync("E_SceneGame_1");
                callback = () => {
                    EasyTouchPlugin.Inst.Enable(true);
                    Net.SpwanPlayer();
                };
                break;
            case SceneName.F_SceneGame_2:
                op = SceneManager.LoadSceneAsync("F_SceneGame_2");
                callback = () => {
                    AssetPoolManager.Inst.FindAsset<AssetScriptableObject, PanelInfo>("parameter", "PanelInfo", (obj)=>{
                            foreach(var panelName in obj.PanelNameList)
                            {
                                Debug.Log("PanelName: " + panelName);
                                UIManager.Inst.InstantiatePanel(panelName);
                            }
                            });
                };
                break;
        }
        StartCoroutine(Loading(op, callback, ui));
    }

    private IEnumerator Loading(AsyncOperation op, Action callback, PanelLoading ui)
    {
        while (op.progress < 0.8)
        {
            yield return null;
            ui.Loading(op.progress);
        }
        ui.Loading(1);
        while (!op.isDone)
            yield return null;
        callback.Invoke();
    }
}
