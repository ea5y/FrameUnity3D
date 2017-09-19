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
    public class ShareCharaController : MonoBehaviour//Singleton<ShareCharaController>
    {
        public CharacterSyncData UserData;
        private CharacterState _state;


        private void Awake()
        {
            //base.GetInstance();
            _state = new CharacterState(GetComponentInChildren<Animator>());;
        }

        private void Update()
        {
            //Net.Net.SyncPlayerPosition(transform.position.x, transform.position.y, transform.position.z);
        }

        public void Trans(double x, double y, double z)
        {
            Debug.Log(string.Format("Pos: ({0},{1},{2})", x,y,z));
            transform.localPosition = new Vector3((float)x, (float)y, (float)z);
            
            //Vector3 offset = new Vector3((float)x,(float)y,(float)z);
            //transform.LookAt(CharaController.Inst.MainCamera.transform.TransformVector(offset) + transform.position);
            //transform.Translate(transform.forward * Time.deltaTime * 0.05f, Space.World);
        }

        public void Trans(SyncPositionData data)
        {
            var pos = new Vector3((float)data.PosX, (float)data.PosY, (float)data.PosZ);
            transform.localPosition = pos;

            var dir = new Vector3((float)data.DirX, 0, (float)data.DirZ);
            transform.LookAt(dir + transform.position);
            //transform.Translate(transform.forward * Time.deltaTime * 0.05f, Space.World);
        }

        public void SetState(SyncStateData data)
        {
            Debug.Log("SyncState: " + data.State);
            switch(data.State)
            {
                case "stand":
                    this.Stand();
                    break;
                case "idle":
                    this.Idle();
                    break;
                case "move":
                    this.Move();
                    break;
                case "attack":
                    this.Attack();
                    break;
                case "skill_1":
                    this.Skill_1();
                    break;
                case "death":
                    this.Death();
                    break;
                case "damage":
                    this.Damage();
                    break;
            }
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
