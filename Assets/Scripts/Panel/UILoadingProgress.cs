//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-25 20:47
//================================

using UnityEngine;
using System.Collections;

public enum LoadingType
{
	Resource,
	Scene
}

public static class LoadingSceneData
{
	public static string nextScene;
	public static LoadingType type;
}

[System.SerializableAttribute]
public class LoadingProgressView
{
	public ProgressForLoadResource progressForLoadResource = 
		new ProgressForLoadResource();
	public ProgressForLoadScene progressForLoadScene = 
		new ProgressForLoadScene();
	
	[System.SerializableAttribute]
	public class ProgressForLoadScene
	{
		public UISlider prgBar;
		public UILabel lblProcess;
	}

	[System.SerializableAttribute]
	public class ProgressForLoadResource
	{
		public UISlider singleSdProgress;
		public UILabel singleLblProgress;
		public UISlider totalSdProgress;
		public UILabel totalLblProgress;
	}
}

public class UILoadingProgress : Singleton<UILoadingProgress>
{
	public LoadingProgressView view = new LoadingProgressView();

	private void Awake()
	{
		Debug.Log("===>Enter LoadingScene");
		if(LoadingSceneData.type == LoadingType.Resource)
		{
			///StartCoroutine(this.LoadResource());
			this.LoadResource();
		}
		else
		{
			//StartCoroutine(this.LoadScene());
		}
	}

	public void Loading(float amount)
	{
		/*
		Debug.Log("Progress: " + amount);
		this.prgBar.value = amount;
		this.lblProcess.text = amount * 100 + "%";
		*/
	}

	private IEnumerator LoadScene()
	{
		var operation =	Application.LoadLevelAsync(LoadingSceneData.nextScene);

		while(!operation.isDone)	
		{
			yield return null;
			this.Loading(operation.progress);
		}
	}

	private void LoadResource()
	{
		/*
		using(WWW www = WWW.LoadFromCacheOrDownload(URL.ASSETBUNDLE_HOST_URL + "Android", 0))
		{
			if(!www.isDone)
			{
				this.Loading(www.progress);
				yield return null;
			}
			Debug.Log("Loading done");
		}
		*/

		ResourceManager.Instance.UpdateResource(this);
		
	}
}
