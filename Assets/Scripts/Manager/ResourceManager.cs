using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public class ResourceManager : Singleton<ResourceManager>
{
    private void Awake()
    {
        this.Load();
    }

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
