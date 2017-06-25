//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-25 20:52
//================================

using UnityEngine;
using System.Collections;
using System;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance{get;set;}
	private static readonly object obj = new object();
	public static T Instance
	{
		get
		{
			lock(obj)
			{
			if(instance == null)
			{
				var singleton = new GameObject();
				instance = singleton.AddComponent<T>();
			}
			return instance;
			}
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
