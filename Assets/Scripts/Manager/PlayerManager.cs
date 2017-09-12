//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-12 14:51
//================================

using UnityEngine;
using Easy.FrameUnity.Net;
using System.Collections.Generic;

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
            var msg = "A player is spawned";
            var go = Instantiate(this.SharePlayer, this.SharePlayerSpawn.transform);
            this.PlayerList.Add(go);

            Debug.Log(msg);
        }
    }
}

