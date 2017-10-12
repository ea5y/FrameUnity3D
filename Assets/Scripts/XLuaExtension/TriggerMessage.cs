//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-12 14:25
//================================

using UnityEngine;
namespace Easy.FrameUnity.XLuaExtension
{
    public class TriggerMessage : Message<TriggerMessage>
    {
        public class TriggerEvent : OnMessageEvent<Collider>{}

        public TriggerEvent onTriggerEnter = new TriggerEvent();
        public TriggerEvent onTriggerStay = new TriggerEvent();
        public TriggerEvent onTriggerExit = new TriggerEvent();

        private void OnTriggerEnter(Collider collider)
        {
            this.onTriggerEnter.Invoke(collider);
        }

        private void OnTriggerStay(Collider collider)
        {
            this.OnTriggerStay(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            this.OnTriggerExit(collider);
        }
    }
}
