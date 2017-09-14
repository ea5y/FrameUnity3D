//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-12 14:51
//================================

using UnityEngine;
using Easy.FrameUnity.Net;
using System.Collections.Generic;
using Easy.FrameUnity.Controller;
using Easy.FrameUnity.Model;

namespace Easy.FrameUnity.Manager
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        public GameObject PersonPlayerSpawn;
        public GameObject SharePlayerSpawn;
        public GameObject PersonalPlayer;
        public GameObject SharePlayer;

        public List<GameObject> PlayerList = new List<GameObject>();

        private void Awake()
        {
            base.GetInstance();
        }

        public void Spawn(CharacterSyncData data)
        {
            if(data.UserId == Player.Inst.UserData.UserId)
            {
                this.SpawnPersonalPlayer(data);
            }
            else
            {
                this.SpawnSharePlayer(data);
            }
        }

        public void SpawnPersonalPlayer(CharacterSyncData data)
        {
            var go = Instantiate(this.PersonalPlayer, this.PersonPlayerSpawn.transform);
            PlayerCameraController.Inst.BindPlayer(go);
        }

        public void SpawnSharePlayer(CharacterSyncData data)
        {
            var msg = "A player is spawned";
            var go = Instantiate(this.SharePlayer, this.SharePlayerSpawn.transform);
            this.PlayerList.Add(go);

            Debug.Log(msg);
        }
    }
}

