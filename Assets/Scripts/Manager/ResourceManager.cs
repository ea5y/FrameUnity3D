using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using LitJson;
using System.ComponentModel;
using System.Security.Cryptography;

public class BundleFile
{
    public string name;
    public string md5;
}

public class BundleFileList
{
    public List<BundleFile> bundleFileList = new List<BundleFile>();
}

public class ResourceManager : Singleton<ResourceManager>
{
	private UILoadingProgress ui;

    private float fileCounter = 0;
    private float fileTotal = 0;

    private List<string> willLoadedList = new List<string>();
	private Queue<Action> asyncQueue = new Queue<Action>();

    private void Awake()
    {
        base.GetInstance();
    }

	private void Update()
	{
		lock(this.asyncQueue)
		{
			if(this.asyncQueue != null && this.asyncQueue.Count > 0)
			{
				Debug.Log("+1");
				var action = this.asyncQueue.Dequeue();
				action();
			}
		}
	}

	public void UpdateResource(UILoadingProgress ui)
	{
        StartCoroutine(_UpdateResource(ui));
	}

    private bool _isLoadingCompleted = false;
    private IEnumerator _UpdateResource(UILoadingProgress ui)
    {
		this.ui = ui;
		var url = URL.ASSETBUNDLE_HOST_URL + "BundleFileList.json";
		var bundleFileListHost = this.LoadBundleFileList(url);
        var result = this.CheckAndFilterBundleFile(bundleFileListHost, URL.RELATIVE_STREAMINGASSETS_URL);
		if(result)
		{
			//load
            ui.SetUI(LoadingType.Resource);
			this.CreateWebClient();
		}
        else
        {
            _isLoadingCompleted = true;
        }

        while (!_isLoadingCompleted)
            yield return null;

        this.OnLoadingCompleted();
    }

    public bool IsUpdate()
    {
		var url = URL.ASSETBUNDLE_HOST_URL + "BundleFileList.json";
		var bundleFileListHost = this.LoadBundleFileList(url);
        var result = this.CheckAndFilterBundleFile(bundleFileListHost, URL.RELATIVE_STREAMINGASSETS_URL);
        return result;
    }

    public BundleFileList LoadBundleFileList(string url)
	{
        //load
		Debug.Log("===>LoadingBundleFileList:");
        Debug.Log("BundleHostURL: " + url);
        WebClient wc = new WebClient();
        Stream s = wc.OpenRead(url);
        StreamReader sr = new StreamReader(s);

        string strLine = sr.ReadToEnd();
        var bundleFileListHost = JsonMapper.ToObject<BundleFileList>(strLine);

		sr.Close();
		s.Close();
		s.Dispose();
		wc.Dispose();
		return bundleFileListHost;
	}

	//Not perfect, should be change
	public bool CheckAndFilterBundleFile(BundleFileList bundleFileListHost, BundleFileList bundleFileListLocal)
	{
		Debug.Log("===>CheckAndFilterBundleFile:");
        foreach(var fileHost in bundleFileListHost.bundleFileList)
        {

            var counter = 0;
            foreach(var fileLocal in bundleFileListLocal.bundleFileList)
            {
                if(fileHost.name == fileLocal.name)
                {
                    //Check md5
                    if(fileHost.md5 != fileLocal.md5)
                    {
                        this.willLoadedList.Add(fileHost.name);
                    }
                }
                else
                {
                    counter += 1;
                }
            }

            if(counter >= bundleFileListLocal.bundleFileList.Count)
            {
                this.willLoadedList.Add(fileHost.name);
            }
        }

		return this.willLoadedList.Count > 0;
	}

    public bool CheckAndFilterBundleFile(BundleFileList bundleFileListHost, string projBundlePath)
    {
		Debug.Log("===>CheckAndFilterBundleFile:");
        var folder = new DirectoryInfo(projBundlePath);
        FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();

        foreach(var fileHost in bundleFileListHost.bundleFileList)
        {
            if (fileHost.name == "BundleFileList.json")
                continue;
            var counter = 0;
            foreach(var fileInfo in fileInfos)
            {
                if(fileHost.name == fileInfo.Name)
                {
                    Debug.Log("Local bundle name: " + fileInfo.Name);
                    //check md5
                    var md5 = new MD5CryptoServiceProvider();
                    var localFile = new FileStream(fileInfo.FullName, FileMode.Open);
                    var localFileMd5 = BitConverter.ToString(md5.ComputeHash(localFile));
                    Debug.Log(string.Format("HostMd5: {0}\nLocalMd5: {1}", fileHost.md5, localFileMd5));
                    if(fileHost.md5 != localFileMd5)
                    {
                        this.willLoadedList.Add(fileHost.name);
                    }
                }
                else
                {
                    counter += 1;
                }
            }

            if(counter >= fileInfos.Length)
            {
                Debug.Log("Counter Max");
                this.willLoadedList.Add(fileHost.name);
            }
        }

		return this.willLoadedList.Count > 0;
    }

	private void OnLoadingCompleted()
	{
		Debug.Log("Load completed.");
        //ScenesManager.Inst.EnterScene(SceneName.F_SceneGame_2);
        ScenesManager.Inst.EnterLoadingScene(SceneName.F_SceneGame_2);
	}

    private void CreateWebClient()
    {
		this.fileTotal = this.willLoadedList.Count;
        this.InitUI();
        foreach(var fileName in this.willLoadedList)
        {
            WebClient wc = new WebClient();

            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.OnDownloadProgressChanged);
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(this.OnDownloadFileCompleted);

			Debug.Log("fileName:" + fileName);
            wc.DownloadFileAsync(new Uri(URL.ASSETBUNDLE_HOST_URL + fileName), URL.RELATIVE_STREAMINGASSETS_URL+ fileName);
        }
    }

    private void InitUI()
    {
        this.ui.view.progressForLoadResource.totalSdProgress.value = 0;
        this.ui.view.progressForLoadResource.totalLblProgress.text = string.Format("Completed:{0}% {1}/{2}(File count)"
                , 0
                , 0
                , this.fileTotal);
    }

    private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
		
		Action oc = ()=>{
		var sd = this.ui.view.progressForLoadResource.singleSdProgress;
		var lbl = this.ui.view.progressForLoadResource.singleLblProgress;
		var v = (float)e.ProgressPercentage / 100;
		Debug.Log("ChangeValue:" + v);
        this.ui.view.progressForLoadResource.singleSdProgress.value = (float)e.ProgressPercentage / 100;
        this.ui.view.progressForLoadResource.singleLblProgress.text = string.Format("Loading file:\n	progress:{0}% {1}/{2}(byte)"
                , e.ProgressPercentage
                , e.BytesReceived
                , e.TotalBytesToReceive);
		};

        this.InvokeAsync(oc);
    }

    private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
		Action oc = ()=>{

			this.fileCounter++;
			var percent = (float)(this.fileCounter / this.fileTotal);

			Debug.Log("CompleteValue:" + percent);
			this.ui.view.progressForLoadResource.totalSdProgress.value = percent;
			this.ui.view.progressForLoadResource.totalLblProgress.text = string.Format("Completed:{0}% {1}/{2}(File count)"
					, percent * 100
					, this.fileCounter
					, this.fileTotal);

            if (percent >= 1)
                _isLoadingCompleted = true;
		};

        this.InvokeAsync(oc);
		
        if(sender is WebClient)
        {
            ((WebClient)sender).CancelAsync();
            ((WebClient)sender).Dispose();
        }
    }

	private void InvokeAsync(Action action)
	{
		lock(this.asyncQueue)
		{
			this.asyncQueue.Enqueue(action);
		}
	}
}
