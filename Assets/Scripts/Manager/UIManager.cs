//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-09 16:10
//================================

using UnityEngine;
using System.Collections;
using Easy.FrameUnity.EsAssetBundle;
using Easy.FrameUnity.ScriptableObj;

namespace Easy.FrameUnity.Manager
{
    public class UIManager : Singleton<UIManager>
    {
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
            AssetPoolManager.Inst.FindAsset<AssetScriptableObject, SObjPrefab>(prefabName, "Panel", (obj)=>{
                    Debug.Log("Instantiate panel: " + obj.Prefab);
                    var go = Instantiate(obj.Prefab);
                    go.transform.parent = this.UIRoot.transform;
                    go.transform.localScale = new Vector3(1, 1, 1);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    });
        }
    }
}
