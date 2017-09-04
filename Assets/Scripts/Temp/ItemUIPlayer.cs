//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-04 20:01
//================================

using UnityEngine;
public class ItemUIPlayer : MonoBehaviour
{
    public UISlider HealthBar;
    private GameObject _followTarget;


    public void SetFollowTarget(GameObject followTarget)
    {
        _followTarget = followTarget;
    }

    public void FollowTarget()
    {
        var screenPos = Camera.main.WorldToScreenPoint(_followTarget.transform.position);
        var uiPos = new Vector3(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2, 0);
        
        transform.localPosition = uiPos;
    }

    private void Update()
    {
        if(_followTarget != null)
            FollowTarget();
    }
}
