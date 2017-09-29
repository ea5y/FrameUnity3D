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
	private Queue<Action> _displayQueue = new Queue<Action>();
    private Queue<Loader> _loaderQueue = new Queue<Loader>();
    private object _asyncLoader = new object();

    private long _recieveBytesLength = 0;
    public long RecieveBytesLength
    {
        get
        {
            return _recieveBytesLength;
        }
        set
        {
            _recieveBytesLength = value;
        }
    }

    private void Awake()
    {
        base.GetInstance();
    }

	private void Update()
	{
		lock(_displayQueue)
		{
			if(this._displayQueue.Count > 0)
			{
				var action = this._displayQueue.Dequeue();
				action.Invoke();
			}
		}

        lock(_asyncLoader)
        {
            if(_loaderQueue.Count > 0)
            {
                var loader = _loaderQueue.Peek();

                if (!loader.IsLoading)
                    loader.Start();
                if (loader.IsCompleted)
                    _loaderQueue.Dequeue();
            }
        }
	}

    public void UpdateResource(UILoadingProgress ui)
	{
		this.ui = ui;
        //StartCoroutine(_UpdateResource(ui));
        StartCoroutine(_UpdateResourceNew(ui));
	}

    private bool _isLoadingCompleted = false;
    private IEnumerator _UpdateResource(UILoadingProgress ui)
    {
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

    private IEnumerator _UpdateResourceNew(UILoadingProgress ui)
    {
        if(IsUpdate())
        {
            LoadResource();
        }
        while (_loaderQueue.Count > 0 || this._displayQueue.Count > 0 )
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

    private void LoadResource()
    {
        ui.SetUI(LoadingType.Resource);
        this.InitUI();
        foreach(var fileName in this.willLoadedList)
        {
            Loader loader = new Loader(URL.ASSETBUNDLE_HOST_URL + fileName, URL.RELATIVE_STREAMINGASSETS_URL + fileName, this.ui);
            _loaderQueue.Enqueue(loader);
        }
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
        ScenesManager.Inst.EnterLoadingScene(SceneName.F_SceneGame_2);
        //ScenesManager.Inst.EnterLoadingScene(SceneName.C_SceneLogin);
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

	public void InvokeAsync(Action action)
	{
		lock(this._displayQueue)
		{
			this._displayQueue.Enqueue(action);
		}
	}


}

public class Loader
{
    public WebClient WebClient;
    public string HostFileURL;
    public string LocalSaveURL;
    public UILoadingProgress ui;

    public bool IsCompleted = false;
    public bool IsLoading = false;
    public Action Callback = ()=> { };
    public Loader(string hostFileURL, string LocalSaveURL, UILoadingProgress ui)
    {
        this.HostFileURL = hostFileURL;
        this.LocalSaveURL = LocalSaveURL;
        this.ui = ui;

        this.WebClient = new WebClient();
        this.WebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.OnDownloadProgressChanged);
        this.WebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.OnDownloadFileCompleted);
    }

    public void Start()
    {
        this.IsLoading = true;
        this.WebClient.DownloadFileAsync(new Uri(HostFileURL), LocalSaveURL);
    }

    private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
		Action oc = ()=>{
		var sd = this.ui.view.progressForLoadResource.singleSdProgress;
		var lbl = this.ui.view.progressForLoadResource.singleLblProgress;
		var v = (float)e.ProgressPercentage / 100;
		//Debug.Log("ChangeValue:" + v);
        this.ui.view.progressForLoadResource.singleSdProgress.value = (float)e.ProgressPercentage / 100;
        this.ui.view.progressForLoadResource.singleLblProgress.text = string.Format("Loading file:\n	progress:{0}% {1}/{2}(byte)"
                , e.ProgressPercentage
                , e.BytesReceived
                , e.TotalBytesToReceive);
		};

        ResourceManager.Inst.InvokeAsync(oc);
    }

    private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        this.IsCompleted = true;
        Debug.Log("File download completed!");
        ResourceManager.Inst.InvokeAsync(Callback);
		
        if(sender is WebClient)
        {
            ((WebClient)sender).CancelAsync();
            ((WebClient)sender).Dispose();
        }
    }
}
