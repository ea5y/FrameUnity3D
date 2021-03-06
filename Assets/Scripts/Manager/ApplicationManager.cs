//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-08 16:24
//================================

using UnityEngine;
using System;
using System.IO;
using XLua;

namespace Easy.FrameUnity.Manager
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public GameObject Managers;
        public GameObject Cameras;
        public GameObject Plugins;

        private void Awake()
        {
            base.GetInstance();
            this.ApplicationEnter();
        }

        private void Start()
        {
            ScenesManager.Inst.EnterLoadingScene(SceneName.B_SceneLoading, LoadingType.Resource);
        }

        private void ApplicationEnter()
        {
            DontDestroyOnLoad(Managers);			
            DontDestroyOnLoad(Cameras);
            DontDestroyOnLoad(Plugins);
        }

        private void ApplicationPause()
        {
        }

        private void ApplicationQuit()
        {
        }
    }
}
