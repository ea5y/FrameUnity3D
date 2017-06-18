//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-18 11:13
//================================

using UnityEngine;
public class BundleManager : MonoBehaviour
{
	public static BundleManager instance;
	public static BundleManager Instance
	{
		get
		{
			if(instance == null)
				instance = new BundleManager();
			return instance;
		}
	}
	public BundleIcon Icon{ get { return new BundleIcon(); } }
	public BundleModel Model{ get { return new BundleModel(); } }
	public BundlePrefab Prefab{ get { return new BundlePrefab(); } }
	public BundleTexture Texture{ get { return new BundleTexture(); } } 
	public BundleScene Scene{ get { return new BundleScene(); } }
	public BundleAudio Audio{ get { return new BundleAudio(); } }
}
