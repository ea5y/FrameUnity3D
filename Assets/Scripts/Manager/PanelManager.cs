//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-11 10:06
//================================

using UnityEngine;
using System.Collections.Generic;
using Easy.FrameUnity.Base;

namespace Easy.FrameUnity.Manager
{
    public class PanelManager : Singleton<PanelManager>
    {
        private Stack<GameObject> _stack = new Stack<GameObject>();

        public void Open(GameObject panelGameObj)
        {
            this.HidePrev();
            panelGameObj.SetActive(true);
            _stack.Push(panelGameObj);
        }

        private void HidePrev()
        {
            if(_stack.Count > 0)
            {
                var panel = _stack.Peek();
                panel.SetActive(false);
            }
        }

        public void Home()
        {
            while(true)
            {
                var panel = _stack.Peek();
                if(panel.name == "PanelMain")
                {
                    panel.SetActive(true);
                    break;
                }
                else
                {
                    panel.SetActive(false);
                    _stack.Pop();
                }
            }
        }

        public void Back()
        {
            var cur = _stack.Pop();
            cur.SetActive(false);

            var pre = _stack.Peek();
            pre.SetActive(true);

            var panel = cur.GetComponent<IPanel>();
            if(panel != null)
            {
                panel.Back();
            }
            else
            {
                if(pre.GetComponent<IPanel>() != null)
                    Back();
            }
        }
    }
}
