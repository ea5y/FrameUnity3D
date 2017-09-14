//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-12 20:04
//================================

using System;
using Easy.FrameUnity.Net;

namespace Easy.FrameUnity.Model
{
    public class Player
    {
        public UserData UserData;

        private static Player _inst;
        public static Player Inst
        {
            get
            {
                if(_inst == null)
                    _inst = new Player();
                return _inst;
            }
        }
    }

    public class PersonalPlayer : Player
    {
        private UserData UserData;
    }

    public class SharePlayer : Player
    {
        private CharacterSyncData UserData;
    }
}

