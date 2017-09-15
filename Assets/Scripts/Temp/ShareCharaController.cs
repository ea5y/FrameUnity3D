//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-14 14:00
//================================

using System;
using System.Collections;
using UnityEngine;
using Easy.FrameUnity.Net;
namespace Easy.FrameUnity.Controller
{
    public class ShareCharaController : Singleton<ShareCharaController>
    {
        public CharacterSyncData UserData;
        private CharacterState _state;

        private void Awake()
        {
            base.GetInstance();
            _state = new CharacterState(GetComponentInChildren<Animator>());;
        }

        public void Idle()
        {
            _state.Idle();
        }

        public void Stand()
        {
            _state.Stand();
        }

        public void Move()
        {
            _state.Move(0);
        }

        public void Attack()
        {
            _state.Attack();
        }

        public void Skill_1()
        {
            _state.Skill_1();
        }

        public void Death()
        {
            _state.Death();
        }

        public void Damage()
        {
            _state.Damage();
        }
    }
}
