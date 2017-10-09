//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-09 16:10
//================================

using UnityEngine;
using System;

namespace Easy.FrameUnity.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        private string _assetPath = "Prefab";
        //private string _assetNamepre = "panel/panelmain/";

        public GameObject UIRoot
        {
            get
            {
                var obj = GameObject.Find("UI Root");
                return obj;
            }
        }

        private void Awake()
        {
            base.GetInstance();
        }

        public void InstantiatePanel(string prefabName)
        {
            //prefabName = _assetNamepre + prefabName;
            BundleManager.Instance.GetPrefab(_assetPath, prefabName, (obj) => {
                    var go = Instantiate(obj);
                    go.transform.parent = this.UIRoot.transform;
                    go.transform.localScale = new Vector3(1, 1, 1);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    });
        }
    }
}
