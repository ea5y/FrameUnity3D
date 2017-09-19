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
        private Dictionary<int, GameObject> _playerDic = new Dictionary<int, GameObject>();

        private void Awake()
        {
            base.GetInstance();
        }

        public void Spawn(CharacterSyncDataSet dataSet)
        {
            var msg = string.Format("CharaSynData count: {0}", dataSet.CharaSyncDataList.Count);
            Debug.Log(msg);
            foreach(var charaData in dataSet.CharaSyncDataList)
            {
                GameObject player;
                if(!_playerDic.TryGetValue(charaData.UserId, out player))
                {
                    if(charaData.UserId == Player.Inst.UserData.UserId)
                    {
                        player = this.SpawnPersonalPlayer(charaData);
                    }
                    else
                    {
                        player = this.SpawnSharePlayer(charaData);
                    }
                    this.PlayerList.Add(player);
                    this._playerDic.Add(charaData.UserId, player);
                    
                    msg = string.Format("Add Player UserId:{0}", charaData.UserId);
                    Debug.Log(msg);
                }
            }
        }

        public GameObject SpawnPersonalPlayer(CharacterSyncData data)
        {
            var go = Instantiate(this.PersonalPlayer, this.PersonPlayerSpawn.transform);
            PlayerCameraController.Inst.BindPlayer(go);
            return go;
        }

        public GameObject SpawnSharePlayer(CharacterSyncData data)
        {
            var msg = "A player is spawned";
            var go = Instantiate(this.SharePlayer, this.SharePlayerSpawn.transform);
            go.GetComponent<ShareCharaController>().UserData = data;

            Debug.Log(msg);
            return go;
        }

        public void RecyclePlayer(CharacterSyncData data)
        {
            GameObject player;
            if(_playerDic.TryGetValue(data.UserId, out player))
            {
                int counter = -1;
                ShareCharaController contr;
                while(true)
                {
                    counter++;
                    contr = this.PlayerList[counter].GetComponent<ShareCharaController>();
                    if (contr == null)
                        continue;

                    if(contr.UserData.UserId == data.UserId)
                    {
                        this.PlayerList.RemoveAt(counter);
                        break;
                    }
                }

                _playerDic.Remove(data.UserId);

                player.transform.parent = null;
                Destroy(player);
            }
        }

        public void SyncPlayerPosition(SyncPositionDataSet dataSet)
        {
            foreach(var data in dataSet.SyncPositionDataList)
            {
                GameObject player;
                if(this._playerDic.TryGetValue(data.UserId, out player))
                {
                    var contro = player.GetComponent<ShareCharaController>();
                    if(contro != null)
                        //contro.Trans(data.PosX, data.PosY, data.PosZ);
                        contro.Trans(data);
                }
            }
        }

        public void SyncPlayrState(SyncStateDataSet dataSet)
        {
            foreach(var data in dataSet.SyncStateDataList)
            {
                GameObject player;
                if(this._playerDic.TryGetValue(data.UserId, out player))
                {
                    var contro = player.GetComponent<ShareCharaController>();
                    if(contro != null)
                        contro.SetState(data);
                }
            }
        }
    }
}

