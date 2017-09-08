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

        private void Awake()
        {
            base.GetInstance();
            this.ApplicationEnter();
        }

        private void Start()
        {
            ScenesManager.Inst.EnterScene(ScenesName.C_SceneLogin);
        }

        private void ApplicationEnter()
        {
            DontDestroyOnLoad(Managers);			
            DontDestroyOnLoad(Cameras);
        }

        private void ApplicationPause()
        {
        }

        private void ApplicationQuit()
        {
        }
    }
}
