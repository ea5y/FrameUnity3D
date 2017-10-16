//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-25 20:47
//================================

using UnityEngine;
using Easy.FrameUnity.Manager;

public enum LoadingType
{
	Resource,
	Scene
}

public static class LoadingSceneData
{
    public static SceneName NextScene;
	public static string nextScene;
	public static LoadingType type;
}

[System.SerializableAttribute]
public class PanelLoadingView
{
	public ProgressForLoadResource progressForLoadResource = 
		new ProgressForLoadResource();
	public ProgressForLoadScene progressForLoadScene = 
		new ProgressForLoadScene();
	
	[System.SerializableAttribute]
	public class ProgressForLoadScene
	{
        public GameObject Root;
		public UISlider prgBar;
		public UILabel lblProcess;
	}

	[System.SerializableAttribute]
	public class ProgressForLoadResource
	{
        public GameObject Root;
		public UISlider singleSdProgress;
		public UILabel singleLblProgress;
		public UISlider totalSdProgress;
		public UILabel totalLblProgress;
	}
}

public class PanelLoading : Singleton<PanelLoading>
{
	public PanelLoadingView view = new PanelLoadingView();

	private void Awake()
	{
        base.GetInstance();
		Debug.Log("===>Enter LoadingScene");
		if(LoadingSceneData.type == LoadingType.Resource)
		{
			this.LoadResource();
		}
		else
		{
            this.LoadScene();
		}
	}

    public void SetUI(LoadingType type)
    {
        this.view.progressForLoadResource.singleSdProgress.gameObject.SetActive(type == LoadingType.Resource);
        this.view.progressForLoadResource.totalSdProgress.gameObject.SetActive(true);
    }

	public void Loading(float amount)
	{
		this.view.progressForLoadResource.totalSdProgress.value = amount;
        var percent = (float)amount * 100 + "%";
        this.view.progressForLoadResource.totalLblProgress.text = percent; 
		Debug.Log("Progress: " + percent);
	}

	private void LoadResource()
	{
		ResourceManager.Inst.UpdateResource(this);
	}

    private void LoadScene()
    {
        ScenesManager.Inst.RealLoadScene(LoadingSceneData.NextScene, this);
    }
}
