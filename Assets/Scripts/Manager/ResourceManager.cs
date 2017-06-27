using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using LitJson;

public class ResourceManager : Singleton<ResourceManager>
{
    private List<string> willLoadedList = new List<string>();

    private void Awake()
    {
        //this.Load();
		this.UpdateResource();
    }

	/*
    public void Load()
    {
        Debug.Log("LocalURL: " + URL.ASSETBUNDLE_LOCAL_URL);
        var strArr = URL.ASSETBUNDLE_LOCAL_URL.Split('/');
        //var str = strArr[strArr.Length-3] + "\\" + strArr[strArr.Length-2] + "\\";
        var str = "Assets/Bundle/test.png";
        WebClient wc = new WebClient();
        wc.DownloadFile("http://www.xiaoyougame.com/x-world/images/activity.png", str); 

        var hwrq = (HttpWebRequest)WebRequest.Create(new Uri(""));
        
       
    }
	*/

	public void UpdateResource()
	{
		var url = URL.ASSETBUNDLE_HOST_URL + "BundleFileList.json";
		var bundleFileListHost = this.LoadBundleFileList(url);
        var bundleFileListLocal = IOHelper.ReadFromJson<BundleFileList>(URL.ASSETBUNDLE_LOCAL_URL);
		
		var result = this.CheckAndFilterBundleFile(bundleFileListHost, bundleFileListLocal);
		if(result)
		{
			//load
			IOHelper.SaveToJson<BundleFileList>(bundleFileListHost, URL.ASSETBUNDLE_LOCAL_URL);
			this.Load();
		}
		else
		{
			//to next scene
			this.OnLoadingCompleted();
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

	private void OnLoadingCompleted()
	{
		Debug.Log("Load completed.");
	}

	private void Load()
	{
		Debug.Log("===>LoadFile:");
		foreach(var fileName in this.willLoadedList)
		{
			var hostUrl = URL.ASSETBUNDLE_HOST_URL + fileName;
			Debug.Log("URL: " + hostUrl);

			var localUrl = URL.ASSETBUNDLE_LOCAL_URL + fileName;
			WebClient wc = new WebClient();
			wc.DownloadFile(hostUrl, localUrl); 
			wc.Dispose();
		}
	}

    /*
    public void CreateBundleFileList(string inputPath)
    {
        var folder = new DirectoryInfo(inputPath);
        FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();

        List<BundleFile> bundleFileList = new List<BundleFile>();
        foreach(var fileInfo in fileInfos) 
        {
            BundleFile bundleFile = new BundleFile();
            bundleFile.name = fileInfo.Name;
            bundleFile.md5 = "1234567890";

            bundleFileList.Add(bundleFile);
        }

        IOHelper.SaveToJson<List<BundleFile>>(bundleFileList, inputPath);
    }
    */
}
