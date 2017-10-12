using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using LitJson;
using System.ComponentModel;
using System.Security.Cryptography;

public class ResourceFile
{
    public string name;
    public string md5;
    public string length;
    public string type;
}

public class BundleFile : ResourceFile
{
    public BundleFile()
    {
        this.type = "bundle";
    }
}

public class LuaFile : ResourceFile
{
    public LuaFile()
    {
        this.type = "lua";
    }
}


//For map with json
public class ResourceFileList
{
    public List<ResourceFile> resourceFileList = new List<ResourceFile>();
}

public class ResourceFileList<T> where T : ResourceFile
{
    public List<T> resourceFileList = new List<T>();
}

namespace Easy.FrameUnity.Manager
{

    public class ResourceManager : Singleton<ResourceManager>
    {
        private UILoadingProgress ui;

        private List<ResourceFile> _willLoadList = new List<ResourceFile>();
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
                lock (_asyncProgress)
                {
                    _recieveBytesLength = value;
                    var progress = (float)_recieveBytesLength / _totalBytesLength;
                    var percent = (float)progress * 100 + "%";

                    var msg = string.Format("Loading file:\n	progress:{0:P1} {1}/{2}(byte)"
                    , progress
                    , _recieveBytesLength
                    , _totalBytesLength);

                    this.InvokeAsync(() =>
                    {
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
            lock (_displayQueue)
            {
                if (this._displayQueue.Count > 0)
                {
                    var action = this._displayQueue.Dequeue();
                    action.Invoke();
                }
            }

            lock (_asyncLoader)
            {
                if (_loaderQueue.Count > 0)
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
            if (IsUpdate())
            {
                LoadResource();
            }
            while (_loaderQueue.Count > 0 || this._displayQueue.Count > 0)
                yield return null;

            //yield return new WaitForSeconds(3);
            this.OnLoadingCompleted();
        }

        public bool IsUpdate()
        {
            var bundleUrl = URL.ASSETBUNDLE_HOST_URL + URL.RESOURCE_FILE_LIST_FILENAME;
            var bundleFileListHost = this.LoadResourceFileList(bundleUrl);
            var bundleResult = this.CheckAndFilterResourceFile(bundleFileListHost, URL.RELATIVE_STREAMINGASSETS_URL);

            var luaUrl = URL.LUA_HOST_URL + URL.RESOURCE_FILE_LIST_FILENAME;
            var luaFileListHost = this.LoadResourceFileList(luaUrl);
            var luaResult = this.CheckAndFilterResourceFile(luaFileListHost, URL.RELATIVE_STREAMINGASSETS_URL);

            return bundleResult || luaResult;
        }

        private void LoadResource()
        {
            ui.SetUI(LoadingType.Scene);
            string hostFileURL = string.Empty;
            foreach (var file in _willLoadList)
            {
                if (file.type == "bundle")
                    hostFileURL = URL.ASSETBUNDLE_HOST_URL + file.name;
                if (file.type == "lua")
                    hostFileURL = URL.LUA_HOST_URL + file.name;

                Loader loader = new Loader(hostFileURL, URL.RELATIVE_STREAMINGASSETS_URL + file.name, this.ui);
                _loaderQueue.Enqueue(loader);
            }
        }

        public ResourceFileList LoadResourceFileList(string url)
        {
            //load
            Debug.Log("BundleHostURL: " + url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetRandomUri(url));
            request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            request.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            request.Headers.Add(HttpRequestHeader.Expires, "-1");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                var str = reader.ReadToEnd();
                var resourceFileListHost = JsonMapper.ToObject<ResourceFileList>(str);
                return resourceFileListHost;
            }
            /*
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
            */

        }

        public static string GetRandomUri(string uri)
        {
            System.Random rd = new System.Random();
            string rdStr = rd.Next(10).ToString() + rd.Next(10).ToString() + rd.Next(10).ToString();
            string rdUri = uri + "?" + rdStr;
            return rdUri;
        }

        public bool CheckAndFilterResourceFile(ResourceFileList resourceFileListHost, string projResourcePath)
        {
            Debug.Log("===>CheckAndFilterResourceFile:");
            var folder = new DirectoryInfo(projResourcePath);
            FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();

            foreach (var fileHost in resourceFileListHost.resourceFileList)
            {
                if (fileHost.name == "ResourceFileList.json")
                    continue;
                var counter = 0;
                Debug.Log("File length: " + fileHost.length);
                foreach (var fileInfo in fileInfos)
                {
                    if (fileHost.name == fileInfo.Name)
                    {
                        Debug.Log("Local resource name: " + fileInfo.Name);
                        //check md5
                        var localFileMd5 = IOHelper.GetFileMD5(fileInfo.FullName);
                        Debug.Log(string.Format("HostMd5: {0}\nLocalMd5: {1}", fileHost.md5, localFileMd5));
                        if (fileHost.md5 != localFileMd5)
                        {
                            _willLoadList.Add(fileHost);
                            _totalBytesLength += long.Parse(fileHost.length);
                        }
                    }
                    else
                    {
                        counter += 1;
                    }
                }

                if (counter >= fileInfos.Length)
                {
                    _willLoadList.Add(fileHost);
                    _totalBytesLength += long.Parse(fileHost.length);
                }
            }

            return _willLoadList.Count > 0;
        }

        private void OnLoadingCompleted()
        {
            Debug.Log("Load completed.");
            ApplicationManager.Inst.EnableHotFix();
            ScenesManager.Inst.EnterLoadingScene(SceneName.F_SceneGame_2);
            //ScenesManager.Inst.EnterLoadingScene(SceneName.C_SceneLogin);
        }

        public void InvokeAsync(Action action)
        {
            lock (this._displayQueue)
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
        public Action Callback = () => { };
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
            Debug.Log("File " + this.HostFileURL + " download completed!");
            ResourceManager.Inst.InvokeAsync(Callback);

            if (sender is WebClient)
            {
                ((WebClient)sender).CancelAsync();
                ((WebClient)sender).Dispose();
            }
        }
    }
}
