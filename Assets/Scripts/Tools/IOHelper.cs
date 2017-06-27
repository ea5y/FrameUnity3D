using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using System.Text;
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

public static class IOHelper
{
#region Json
    public static void SaveToJson<T>(object obj, string outPath)
    {
        Debug.Log("SaveToJsonTo: " + outPath);
        CheckPath(outPath);

        //to json
        string values = JsonMapper.ToJson(obj);

        //Encrypt
        //@TODO
        
        //write
        var fileName = outPath + "/" + typeof(T).ToString() + ".json";
        Debug.Log("FileName: " + fileName);
        //FileStream fs = new FileStream( fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
        FileStream fs = new FileStream( fileName, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(values);

        //close
        sw.Close();
        fs.Close();
        fs.Dispose();
    }

    public static T ReadFromJson<T>(string fullPath)
    {
        var fileFullName = fullPath + typeof(T) + ".json";
        Debug.Log("JsonFileFullName: " + fileFullName);
        if(File.Exists(fileFullName))
        {
            //read
            FileStream fs = new FileStream(fileFullName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            var values = sr.ReadToEnd();

            //decrypt
            //@TODO
            
            //to obj
            var obj = JsonMapper.ToObject<T>(values);
            return obj;
        }
        else
        {
            Debug.LogError("JsonFile: " + typeof(T).ToString() + "not exists");
            return default(T);
        }
    }

    public static void CheckPath(string path)
    {
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// Rijndael�����㷨
    /// </summary>
    /// <param name="pString">�����ܵ�����</param>
    /// <param name="pKey">��Կ,���ȿ���Ϊ:64λ(byte[8]),128λ(byte[16]),192λ(byte[24]),256λ(byte[32])</param>
    /// <param name="iv">iv����,����Ϊ128��byte[16])</param>
    /// <returns></returns>
    private static string RijndaelEncrypt(string pString, string pKey)
    {
        //��Կ
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
        //��������������
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(pString);

        //Rijndael�����㷨
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        //���ؼ��ܺ������
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// ijndael�����㷨
    /// </summary>
    /// <param name="pString">�����ܵ�����</param>
    /// <param name="pKey">��Կ,���ȿ���Ϊ:64λ(byte[8]),128λ(byte[16]),192λ(byte[24]),256λ(byte[32])</param>
    /// <param name="iv">iv����,����Ϊ128��byte[16])</param>
    /// <returns></returns>
    private static String RijndaelDecrypt(string pString, string pKey)
    {
        //������Կ
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
        //��������������
        byte[] toEncryptArray = Convert.FromBase64String(pString);

        //Rijndael�����㷨
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        //���ؽ��ܺ������
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    public static string GetFileMD5(string fileFullName)
    {
        MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
        //FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        FileStream fs = new FileStream(fileFullName, FileMode.Open);
        byte[] hash = md5Provider.ComputeHash(fs);
        fs.Close();
        fs.Dispose();
        return System.BitConverter.ToString(hash);
    }
    #endregion
}
