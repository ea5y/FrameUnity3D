//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-12 14:22
//================================

using UnityEngine;

namespace Easy.FrameUnity.XLuaExtension
{
    public class UpdateMessage : Message<UpdateMessage>
    {
        public class UpdateEvent : OnMessageEvent{}

        public UpdateEvent update = new UpdateEvent();
        public UpdateEvent fixedUpdate = new UpdateEvent();
        public UpdateEvent lateUpdate = new UpdateEvent();

        private void Update()
        {
            update.Invoke();
        }

        private void FixedUpdate()
        {
            fixedUpdate.Invoke();
        }

        private void LateUpdate()
        {
            lateUpdate.Invoke();
        }
    }
}
