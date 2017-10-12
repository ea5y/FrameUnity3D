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

        public LuaEnv LuaEnv;

        private void Awake()
        {
            base.GetInstance();
            //this.EnableHotFix();
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
        
        public void EnableHotFix()
        {
            if(File.Exists(URL.HOTFIX_URL))
            {
                LuaEnv = new LuaEnv();
                LuaEnv.DoString(File.ReadAllText(URL.HOTFIX_URL));

                //LuaTableManager.Inst.Init();
            }
            /*
            if (File.Exists(URL.HOTFIX_URL))
            {
                LuaEnv luaEnv = new LuaEnv();
                using (FileStream fs = new FileStream(URL.HOTFIX_URL, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader sr = new StreamReader(fs))
                {
                    var values = sr.ReadToEnd();
                    luaEnv.DoString(values);
                }
            }
            */
        }
    }
}
