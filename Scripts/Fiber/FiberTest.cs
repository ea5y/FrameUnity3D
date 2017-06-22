using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FiberTest : MonoBehaviour 
{
    public List<UnityEngine.Object> GameObjectList = new List<UnityEngine.Object>();
    private string assetPath = "prefab";
    private string assetName = "CC";
    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 170), "Create");

        if(GUI.Button(new Rect(20, 40, 180, 60), "Create 1"))
        {

            BundleManager.Instance.GetPrefab(this.assetPath, this.assetName, (obj) => {
                    var go = Instantiate(obj);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    this.GameObjectList.Add(go);
                    });	
        }

        if(GUI.Button(new Rect(20, 110, 180, 60), "Create 1000"))
        {
            for(int i = 1; i < 1001; i++)
            {
                var p = i + i;
                BundleManager.Instance.GetPrefab(this.assetPath, this.assetName, (obj) => {
                        var go = Instantiate(obj);
                        go.transform.localPosition = new Vector3(p, 0, 0);
                        this.GameObjectList.Add(go);
                        });	
            }
        }

        GUI.Box(new Rect(220, 10, 200, 170), "Destroy");

        if(GUI.Button(new Rect(230, 40, 180, 60), "Destroy 1"))
        {
            Destroy(GameObjectList[0]);
            GameObjectList.RemoveAt(0);
        }

        if(GUI.Button(new Rect(230, 110, 180, 60), "Destroy 1000"))
        {
            for(int i = 0; i < 1000; i++)
            {
                //Use UnityEngine.Object.Destroy, not GameObject.Destroy
                Destroy(GameObjectList[0]);
                GameObjectList.RemoveAt(0);
            }
        }

        GUI.Label(new Rect(20, 190, 180, 50), "AssetPath: "); 
        GUI.Label(new Rect(20, 250, 180, 50), "AssetName: "); 

        assetPath = GUI.TextField(new Rect(210, 190, 180, 50), assetPath, 15);
        assetName = GUI.TextField(new Rect(210, 250, 180, 50), assetName, 15);
    }
}
