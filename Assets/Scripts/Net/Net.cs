using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Easy.FrameUnity.Manager;
using Easy.FrameUnity.Util;

namespace Easy.FrameUnity.Net
{
    public static class ActionID
    {
        public static readonly int Login = 1002;
        public static readonly int SpwanPlayer = 1003;

    }

    public class CastID
    {
        public const int SpwanPlayer = 1000;
        public const int RecyclePlayer = 1001;
        public const int SyncPlayerPosition = 1002;
        public const int SyncPlayrState = 1003;
    }

    public class Net : MonoBehaviour
    {
        private static Queue<Action> _dispatchQueue = new Queue<Action>();
        
        private static object _objAsync = new object();

        private void OnDestroy()
        {
            SocketClient.CloseConnection();
            Debug.Log("Quit");
        }

        private void OnApplicationQuit()
        {
        }

        private void Update()
        {
            if(_dispatchQueue.Count > 0)
            {
                Queue<Action> actionQueue = new Queue<Action>();
                Action action;
                lock(_objAsync)
                {
                    while(_dispatchQueue.Count > 0)
                    {
                        action = _dispatchQueue.Dequeue();
                        actionQueue.Enqueue(action);
                    }
                }
                while(actionQueue.Count > 0)
                {
                    action = actionQueue.Dequeue();
                    action.Invoke();
                }
            }
        }

        public static void InvokeAsync(Action action)
        {
            lock(_objAsync)
            {
                _dispatchQueue.Enqueue(()=>{
                        action();
                        });
            }
        }

        public static Dictionary<int, Action<byte[]>> CastDic = new Dictionary<int, Action<byte[]>>()
        {
            {CastID.SpwanPlayer, (res)=>{CastLogin(res);}},
            {CastID.RecyclePlayer, (res)=>{CastRecyclePlayer(res);}}
        };

        private static void CastLogin(byte[] res)
        {
            var obj = ProtoBufUtil.Deserialize<CharacterSyncDataSet>(res);
            Debug.Log("Cast:" + obj);
            PlayerManager.Inst.Spawn(obj);            
        }

        private static void CastRecyclePlayer(byte[] res)
        {
            var obj = ProtoBufUtil.Deserialize<CharacterSyncData>(res);
            PlayerManager.Inst.RecyclePlayer(obj);
        }

        public static void Login(string username, string password, Action<LoginDataRes> callback)
        {
            var data = new RegisterData() { Username = username, Password = password };
            var bytes = PackageFactory.Pack(ActionID.Login, data, callback);
            SocketClient.Send(bytes);
        }

        public static void SpwanPlayer()
        {
            var bytes = PackageFactory.Pack(ActionID.SpwanPlayer);
            SocketClient.Send(bytes);
        }
    }
}
