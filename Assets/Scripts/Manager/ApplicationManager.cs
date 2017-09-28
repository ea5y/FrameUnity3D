//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-08 16:24
//================================

using UnityEngine;

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
            ScenesManager.Inst.EnterLoadingScene(SceneName.C_SceneLogin);
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
