//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-11 09:55
//================================

using UnityEngine;
using System;

namespace Easy.FrameUnity.Base
{
    public interface IPanel
    {
        void Open(object data);
        void OpenChild(object data);
        void Close();
        void Back();
        void Home();
    }

    public interface IPanelChild
    {
    }

    public interface ITween
    {
        void Forward();
        void Reverse();
    }

    public abstract class PanelBase<T> : LuaBehaviour, IPanel where T : LuaBehaviour
    {
        public static T Inst;
        private object _objSync = new object();

        protected void GetInstance()
        {
            lock(_objSync)
            {
                if(Inst == null)
                {
                    Inst = this as T;
                }
            }
        }

        public abstract void Open(object data);

        public virtual void OpenChild(object data)
        {
            Debug.Log("===>Child");
        }

        public abstract void Close();

        public abstract void Back();

        public virtual void Home()
        {
            Debug.Log("===>Home");
        }

        protected void Destroy()
        {
            Debug.Log("Panel Destroy");
            Inst = null;
        }
    }
}
