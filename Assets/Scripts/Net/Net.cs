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
        public static readonly int SyncPlayerPosition = 1004;
        public static readonly int SyncPlayerState = 1005;
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
            {CastID.RecyclePlayer, (res)=>{CastRecyclePlayer(res);}},
            {CastID.SyncPlayerPosition, (res)=>{CastSyncPlayerPosition(res);}},
            {CastID.SyncPlayrState, (res)=>{CastSyncPlayerState(res);}}
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

        private static void CastSyncPlayerPosition(byte[] res)
        {
            var obj = ProtoBufUtil.Deserialize<SyncPositionDataSet>(res);
            //Debug.Log("Cast:" + obj);
            PlayerManager.Inst.SyncPlayerPosition(obj);
        }

        private static void CastSyncPlayerState(byte[] res)
        {
            var obj = ProtoBufUtil.Deserialize<SyncStateDataSet>(res);
            PlayerManager.Inst.SyncPlayrState(obj);
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

        public static void SyncPlayerPosition(double x, double y, double z, double dirX, double dirZ)
        {
            var data = new PositionData(){PosX = x, PosY = y, PosZ = z, DirX = dirX, DirZ = dirZ};
            var bytes = PackageFactory.Pack(ActionID.SyncPlayerPosition, data);
            SocketClient.Send(bytes);
        }

        public static void SyncPlayerState(string state)
        {
            var str = state.ToLower();
            var data = new StateData(){State = str};
            var bytes = PackageFactory.Pack(ActionID.SyncPlayerState, data);
            SocketClient.Send(bytes);
        }
    }
}
