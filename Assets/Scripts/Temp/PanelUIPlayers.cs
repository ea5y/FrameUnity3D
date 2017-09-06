//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-04 19:43
//================================

using UnityEngine;
public class PanelUIPlayers : MonoBehaviour
{
    public static PanelUIPlayers Inst;
    public GameObject UIPlayerPrefab;
    public GameObject Panel;

    private void Awake()
    {
        Inst = this;
    }

    public GameObject AddUIPlayer(GameObject followTarget)
    {
        var go = Instantiate(this.UIPlayerPrefab, this.Panel.transform);
        ItemUIPlayer item = go.GetComponent<ItemUIPlayer>();
        item.SetFollowTarget(followTarget);
        return go;
        //Set follow
        //var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        //Debug.Log("screenPos:" + screenPos);
    }
}
