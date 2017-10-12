//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-12 13:49
//================================

using UnityEngine;

namespace Easy.FrameUnity.XLuaExtension
{
    public class Message<T> : MonoBehaviour where T : Message<T>
    {
        
        public static T Require(GameObject obj)
        {
            T transmitter = obj.GetComponent<T>();
            if(transmitter == null)
            {
                transmitter = obj.AddComponent<T>();
            }
            return transmitter;
        }

        public static void Dismiss(GameObject obj)
        {
            if(obj == null)
                return;
            T message = obj.GetComponent<T>();
            if(message == null)
                return;
            message.Dismiss();
        }

        public void Dismiss()
        {
            Destroy(this);
        }
    }

    public delegate void OnMessageAction();
    public class OnMessageEvent
    {
        private event OnMessageAction _event;

        public void AddAction(OnMessageAction action)
        {
            _event += action;
        }

        public void RemoveAction(OnMessageAction action)
        {
            _event -= action;
        }

        public void Clear()
        {
            _event = null;
        }

        public void Invoke()
        {
            if(_event != null)
                _event.Invoke();
        }
    }

    public delegate void OnMessageAction<T>(T arg);
    public class OnMessageEvent<T>
    {
        private event OnMessageAction<T> _event;

        public void AddAction(OnMessageAction<T> action)
        {
            _event += action;
        }

        public void RemoveAction(OnMessageAction<T> action)
        {
            _event -= action;
        }

        public void Clear()
        {
            _event = null;
        }

        public void Invoke(T arg)
        {
            if(_event != null)
                _event.Invoke(arg);
        }
    }
}
