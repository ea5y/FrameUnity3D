using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FiberTest : MonoBehaviour 
{
    public List<UnityEngine.Object> GameObjectList = new List<UnityEngine.Object>();

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 90), "Create");

        if(GUI.Button(new Rect(20, 40, 80, 20), "Create 1"))
        {

            BundleManager.Instance.GetPrefab("", "prefab", (obj) => {
                    var go = Instantiate(obj);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    this.GameObjectList.Add(go);
                    });	
        }

        if(GUI.Button(new Rect(20, 70, 80, 20), "Create 10"))
        {
            for(int i = 1; i < 1001; i++)
            {
                var p = i + i;
                BundleManager.Instance.GetPrefab("", "prefab", (obj) => {
                        var go = Instantiate(obj);
                        go.transform.localPosition = new Vector3(p, 0, 0);
                        this.GameObjectList.Add(go);
                        });	
            }
        }

        GUI.Box(new Rect(120, 10, 100, 90), "Destroy");

        if(GUI.Button(new Rect(130, 40, 80, 20), "Destroy 1"))
        {
            GameObject.Destroy(GameObjectList[0]);
            GameObjectList.RemoveAt(0);
        }

        if(GUI.Button(new Rect(130, 70, 80, 20), "Destroy 100"))
        {
            for(int i = 0; i < 100; i++)
            {
                Destroy(GameObjectList[0]);
                //GameObject.Destroy(GameObjectList[0]);
                GameObjectList.RemoveAt(0);
            }
        }
    }
}
