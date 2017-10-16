using System;
using System.IO;
using LitJson;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace Easy.FrameUnity.Util
{
    public static class IOHelperUtil
    {
        #region Json
        public static void SaveToJson<T>(object obj, string projectPath)
        {
            var strArray = typeof(T).ToString().Split('.');
            var fileName = strArray[strArray.Length - 1];
            var fileProjectFullName = projectPath + fileName + ".json";
            var msg = string.Format("===>Save to Json file:\n {0}", fileProjectFullName);
            Debug.Log(msg);
            //Console.WriteLine(msg);
            //Debug.Print(msg);

            //to json
            string values = JsonMapper.ToJson(obj);

            //Encrypt
            //@TODO

            //write
            CheckPath(projectPath);
            using (FileStream fs = new FileStream(fileProjectFullName, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(values);
            }
        }

        public static T ReadFromJson<T>(string projectPath) where T : new()
        {
            var strArray = typeof(T).ToString().Split('.');
            var fileName = strArray[strArray.Length - 1];
            var fileProjectFullName = projectPath + fileName + ".json";

            var msg = string.Format("===>Read fram Json file:\n {0}", fileProjectFullName);
            //Console.WriteLine(msg);
            Debug.Log(msg);

            if (!File.Exists(fileProjectFullName))
            {
                var msg1 = string.Format("{0}.json not exists!", fileName);
                //Console.WriteLine(msg1);
                Debug.Log(msg1);

                SaveToJson<T>(new T(), projectPath);
            }

            //read
            using (FileStream fsr = new FileStream(projectPath + fileName + ".json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fsr))
            {
                var values = sr.ReadToEnd();
                //decrypt
                //@TODO

                //to obj
                var obj = JsonMapper.ToObject<T>(values);
                return obj;
            }
        }

        public static void CheckPath(string path)
        {
            if (!Directory.Exists(path))
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
            using (FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] hash = md5Provider.ComputeHash(fs);
                return System.BitConverter.ToString(hash);
            }
        }

        //@TODO
        //Write a common function for read & write file otherwise will happen Sharing violation 
        #endregion
    }
}

