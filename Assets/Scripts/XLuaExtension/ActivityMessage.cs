//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-12 15:23
//================================

using UnityEngine;
namespace Easy.FrameUnity.XLuaExtension
{
    public class ActivitMessage : Message<ActivitMessage>
    {
        public class ActivityEvent : OnMessageEvent{}

        public ActivityEvent awake = new ActivityEvent();
        public ActivityEvent start = new ActivityEvent();
        public ActivityEvent onEnable = new ActivityEvent();
        public ActivityEvent onDisable = new ActivityEvent();
        public ActivityEvent onDestroy = new ActivityEvent();
            
        private void Awake()
        {
            this.awake.Invoke();
        }

        private void Start()
        {
            this.start.Invoke();
        }

        private void OnEnable()
        {
            this.onEnable.Invoke();
        }

        private void OnDisable()
        {
            this.onDisable.Invoke();
        }

        private void OnDestroy()
        {
            this.onDestroy.Invoke();
        }
    }
}
