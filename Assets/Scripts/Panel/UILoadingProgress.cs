//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-25 20:47
//================================

using UnityEngine;
using System.Collections;

namespace TOOLS
{
	public enum LoadingType
	{
		Resource,
		Scene
	}
}

public static class LoadingProgressData
{
	public static string nextScene;
	public static TOOLS.LoadingType type;
}

public class UILoadingProgress : Singleton<UILoadingProgress>
{
	public UISlider prgBar;
	public UILabel lblProcess;

	public string nextScene;
	public TOOLS.LoadingType type;

	/*
	private void Awake()
	{
		Debug.Log("===>Enter LoadingScene");
	}
	*/
	private void Awake()
	{
		Debug.Log("===>Enter LoadingScene");
		if(LoadingProgressData.type == TOOLS.LoadingType.Resource)
		{
			StartCoroutine(this.LoadResource());
		}
		else
		{
			StartCoroutine(this.LoadScene());
		}
	}

	public void Loading(float amount)
	{
		Debug.Log("Progress: " + amount);
		this.prgBar.value = amount;
		this.lblProcess.text = amount * 100 + "%";
	}

	private IEnumerator LoadScene()
	{
		var operation =	Application.LoadLevelAsync(LoadingProgressData.nextScene);

		while(!operation.isDone)	
		{
			yield return null;
			this.Loading(operation.progress);
		}
	}

	private IEnumerator LoadResource()
	{
		using(WWW www = WWW.LoadFromCacheOrDownload(URL.ASSETBUNDLE_HOST_URL + "Android", 0))
		{
			if(!www.isDone)
			{
				this.Loading(www.progress);
				yield return null;
			}
			Debug.Log("Loading done");
		}
	}
}
