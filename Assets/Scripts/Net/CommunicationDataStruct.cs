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

    [ProtoContractAttribute]
    public class CharacterSyncDataSet
    {
        [ProtoMemberAttribute(1)]
        public List<CharacterSyncData> CharaSyncDataList;
    }

    //==========Sync Position Data========
    [ProtoContractAttribute]
    public class SyncPositionDataSet
    {
        [ProtoMemberAttribute(1)]
        public List<SyncPositionData> SyncPositionDataList;
    }

    [ProtoContractAttribute]
    public class SyncPositionData 
    {
        [ProtoMemberAttribute(1)]
        public int UserId;
        [ProtoMemberAttribute(2)]
        public double PosX;
        [ProtoMemberAttribute(3)]
        public double PosY;
        [ProtoMemberAttribute(4)]
        public double PosZ;

        [ProtoMemberAttribute(5)]
        public double DirX;
        [ProtoMemberAttribute(6)]
        public double DirZ;
    }

    public class PositionData : BaseReqData
    {
        public double PosX;
        public double PosY;
        public double PosZ;

        public double DirX;
        public double DirZ;
    }

    public class StateData : BaseReqData
    {
        public string State;
    }

    [ProtoContractAttribute]
    public class SyncStateDataSet
    {
        [ProtoMemberAttribute(1)]
        public List<SyncStateData> SyncStateDataList;
    }

    [ProtoContractAttribute]
    public class SyncStateData
    {
        [ProtoMemberAttribute(1)]
        public int UserId;
        [ProtoMemberAttribute(2)]
        public string State;
    }
}
