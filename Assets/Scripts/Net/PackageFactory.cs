using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Easy.FrameUnity.Net
{
    public class PackageReqHead
    {
        public int MsgId = 0;
        public int ActionId = 0;

        public static int UserId = 0;
        public static string SessionId = "";
        public Action<string> callback;
    }

    public class PackageResHead
    {
        public int StatusCode;
        public string Description;
        public int ActionId;
        public int MsgId;
        public string SessionId;
        public int UserId;
        public string StrTime;
    }

    public class PackageCastHead
    {
        public int ActionId;
    }

    public class PackageFactory
    {
        private static string _sendStr;
        private static int MsgCounter = 0;

        public static byte[] Pack(int actionId, BaseReqData data)
        {
            var head = new PackageReqHead() { ActionId = actionId, MsgId = ++MsgCounter };
            WriteHead(head);
            WriteData(data);

            Debug.Log(string.Format("Send: {0}", _sendStr));
            var bytes = WriteBytesLength();
            _sendStr = "";
            return bytes;
        }

        public static byte[] Pack<T>(int actionId, BaseReqData data, Action<T> callback) where T : BaseResData
        {
            var head = new PackageReqHead() { ActionId = actionId, MsgId = ++MsgCounter,
                callback = (res)=>
                {
                    var obj = JsonMapper.ToObject<T>(res);
                    Net.InvokeAsync(()=>{ callback(obj); });
                } };
            SocketClient.SendDic.Add(head.MsgId, head);
            WriteHead(head);
            WriteData(data);

            Debug.Log(string.Format("Send: {0}", _sendStr));
            var bytes = WriteBytesLength();
            _sendStr = "";
            return bytes;
        }

        private static void WriteHead(PackageReqHead head)
        {
            _sendStr += string.Format(
                    "MsgId={0}&ActionId={1}&Sid={2}&Uid={3}",
                    head.MsgId, head.ActionId, PackageReqHead.SessionId, PackageReqHead.UserId);
        }

        private static void WriteData(BaseReqData data)
        {
            _sendStr += string.Format("&data={0}", JsonMapper.ToJson(data));
        }

        private static byte[] WriteBytesLength()
        {
            byte[] tempBytes = Encoding.ASCII.GetBytes(_sendStr);
            byte[] len = BitConverter.GetBytes(tempBytes.Length);
            byte[] resultBytes = new byte[tempBytes.Length + len.Length];
            Buffer.BlockCopy(len, 0, resultBytes, 0, len.Length);
            Buffer.BlockCopy(tempBytes, 0, resultBytes, len.Length, tempBytes.Length);
            return resultBytes;
        }

        public static bool Unpack(byte[] data, out PackageCastHead head, out string res)
        {
            head = null;
            res = null;
            int pos = 0;

            head = new PackageCastHead();
            head.ActionId = GetInt(data, ref pos);
            byte[] bodyBytes = new byte[data.Length - 4];
            Buffer.BlockCopy(data, pos, bodyBytes, 0, bodyBytes.Length);
            res = Encoding.UTF8.GetString(bodyBytes);

            return true;
        }

        public static bool Unpack(byte[] data, out PackageResHead head, out string res)
        {
            head = null;
            byte[] bodyBytes;
            res = null;

            int pos = 0;
            int dataLength = GetInt(data, ref pos);
            if (dataLength != data.Length)
            {
                return false;
            }

            head = new PackageResHead();
            head.StatusCode = GetInt(data, ref pos);
            head.MsgId = GetInt(data, ref pos);
            head.Description = GetString(data, ref pos);
            head.ActionId = GetInt(data, ref pos);
            head.StrTime = GetString(data, ref pos);
            //int bodyLen = data.Length - pos;
            int bodyLen = GetInt(data, ref pos);
            if (bodyLen > 0)
            {
                bodyBytes = new byte[bodyLen];
                Buffer.BlockCopy(data, pos, bodyBytes, 0, bodyLen);
                string str = Encoding.UTF8.GetString(bodyBytes);
                //Debug.Log(string.Format("Res: {0}", str));
                //res = JsonMapper.ToObject<BaseResData>(str);
                res = str;
            }
            else
            {
                bodyBytes = new byte[0];
            }
            return true;
        }

        private static int GetInt(byte[] data, ref int pos)
        {
            int val = BitConverter.ToInt32(data, pos);
            pos += sizeof(int);
            return val;
        }

        private static string GetString(byte[] data, ref int pos)
        {
            string val = string.Empty;
            int len = GetInt(data, ref pos);
            if (len > 0)
            {
                val = Encoding.UTF8.GetString(data, pos, len);
                pos += len;
            }
            return val;
        }
    }
}
