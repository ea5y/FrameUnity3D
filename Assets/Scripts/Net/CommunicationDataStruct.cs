using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Easy.FrameUnity.Net
{
    //============BaseData=============
    public class BaseReqData
    {

    }

    public class BaseResData
    {

    }

    public class BaseCastData
    {

    }

    //===========LoginData=============
    public class RegisterData : BaseReqData
    {
        public string Username;
        public string Password;
    }

    public class UserData 
    {
        public int UserId;
        public string NickName;
        public int Hp;
        public double PosX;
        public double PosY;
        public double PosZ;
    }

    public class LoginDataRes : BaseResData
    {
        public string SessionId;
        public UserData UserData;
    }

    [ProtoContractAttribute]
    public class CharacterSyncData
    {
        [ProtoMemberAttribute(1)]
        public int UserId;
        [ProtoMemberAttribute(2)]
        public double PosX;
        [ProtoMemberAttribute(3)]
        public double PosY;
        [ProtoMemberAttribute(4)]
        public double PosZ;
    }
}
