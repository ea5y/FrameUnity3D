//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-29 10:45
//================================

using System;
using UnityEngine;
using XLua;
using Easy.FrameUnity;
using Easy.FrameUnity.Base;
using Easy.FrameUnity.Manager;
using System.Collections.Generic;
using System.Collections;

namespace Easy.FrameUnity.Panel
{
    [HotfixAttribute(HotfixFlag.Stateless)]
    public class PanelMain : PanelBase<PanelMain>
    {
        public Dictionary<string, GameObject> UIDic = new Dictionary<string, GameObject>();
        public PanelMain()
        {
        }

        public override void Back()
        {
        }

        public override void Close()
        {
        }

        public override void Open(object data)
        {
        }

        private void Awake()
        {
            base.GetInstance();
            base.MapHotfixUI();
            Debug.Log("CS: Awake");
        }

        private void Start()
        {
            Debug.Log("CS: Start");
        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
        }
    }
}
