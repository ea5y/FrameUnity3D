//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-03 10:12
//================================

using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}

