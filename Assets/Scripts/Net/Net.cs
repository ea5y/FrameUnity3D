using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Easy.FrameUnity.Net
{
    public static class ActionID
    {
        public static readonly int Login = 1002;
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

        public static void Login(string username, string password, Action<LoginDataRes> callback)
        {
            var data = new RegisterData() { Username = username, Password = password };
            var bytes = PackageFactory.Pack(ActionID.Login, data, callback);
            SocketClient.Send(bytes);
        }
    }
}
