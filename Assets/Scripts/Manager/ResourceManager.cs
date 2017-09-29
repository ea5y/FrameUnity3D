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
    public string length;
}

public class BundleFileList
{
    public List<BundleFile> bundleFileList = new List<BundleFile>();
}

public class ResourceManager : Singleton<ResourceManager>
{
	private UILoadingProgress ui;

    private List<string> willLoadedList = new List<string>();
	private Queue<Action> _displayQueue = new Queue<Action>();
    private Queue<Loader> _loaderQueue = new Queue<Loader>();
    private object _asyncLoader = new object();
    private object _asyncProgress = new object();

    private long _totalBytesLength = 0;
    private long _recieveBytesLength = 0;
    public long RecieveBytesLength
    {
        get
        {
            return _recieveBytesLength;
        }
        set
        {
            lock(_asyncProgress)
            {
                _recieveBytesLength = value;
                var progress = (float)_recieveBytesLength / _totalBytesLength;
                var percent = (float)progress * 100 + "%";

                var msg = string.Format("Loading file:\n	progress:{0:P1} {1}/{2}(byte)"
                , progress 
                , _recieveBytesLength
                , _totalBytesLength);

                this.InvokeAsync(() => {
                    this.ui.view.progressForLoadResource.totalSdProgress.value = progress;
                    this.ui.view.progressForLoadResource.totalLblProgress.text = msg;
                });
            }
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
        StartCoroutine(_UpdateResource(ui));
	}

    private IEnumerator _UpdateResource(UILoadingProgress ui)
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
        ui.SetUI(LoadingType.Scene);
        foreach(var fileName in this.willLoadedList)
        {
            Loader loader = new Loader(URL.ASSETBUNDLE_HOST_URL + fileName, URL.RELATIVE_STREAMINGASSETS_URL + fileName, this.ui);
            _loaderQueue.Enqueue(loader);
        }
    }

    public BundleFileList LoadBundleFileList(string url)
	{
        //load
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetRandomUri(url));
        request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
        request.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
        request.Headers.Add(HttpRequestHeader.Expires, "-1");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using(var responseStream = response.GetResponseStream())
        using(StreamReader reader = new StreamReader(responseStream))
        {
            var str = reader.ReadToEnd();
            var bundleFileListHost = JsonMapper.ToObject<BundleFileList>(str);
            return bundleFileListHost;
        }
	}

    public static string GetRandomUri(string uri)
    {
        System.Random rd = new System.Random();
        string rdStr = rd.Next(10).ToString() + rd.Next(10).ToString() + rd.Next(10).ToString();
        string rdUri = uri + "?" + rdStr;
        return rdUri;
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
            Debug.Log("File length: " + fileHost.length);
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
                        _totalBytesLength += long.Parse(fileHost.length);
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
                _totalBytesLength += long.Parse(fileHost.length);
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

        this.Start();
    }

    public void Start()
    {
        this.IsLoading = true;
        this.WebClient.DownloadFileAsync(new Uri(HostFileURL), LocalSaveURL);
    }

    private long _recieveLength = 0;
    private long _changeValue = 0;
    private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        _changeValue = e.BytesReceived - _recieveLength;
        _recieveLength = e.BytesReceived;
        ResourceManager.Inst.RecieveBytesLength += _changeValue;
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
