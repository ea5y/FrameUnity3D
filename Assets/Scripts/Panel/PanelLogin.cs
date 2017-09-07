//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-07 13:57
//================================

using UnityEngine;
using Easy.FrameUnity.Net;
using Easy.FrameUnity.Login;

    public class LoginData 
    {
        public string Username;
        public string Password;
        public string Sid;
    }

public class PanelLogin : MonoBehaviour//Singleton<PanelLogin>
{
    public UIInput InputUsername;
    public UIInput InputPassword;
    public UIButton BtnLogin;

    public static LoginData _loginData;
    private LoginUnit _loginUnit;

    private void Awake()
    {
        this.RegisterBtnEvent();
        this.ReadLoginData();
        this.CreateLoginUnit();
        this.SetLogin();
    }

    private void ReadLoginData()
    {
        //_loginData = IOHelper.ReadFromJson<LoginData>(URL.ASSETBUNDLE_LOCAL_URL);
        _loginData = IOHelper.ReadFromJson<LoginData>(URL.RELATIVE_STREAMINGASSETS_URL);
        PackageReqHead.SessionId = _loginData.Sid;
    }

    private void CreateLoginUnit()
    {
        var normalLogin = new NormalLogin(_loginData);
        normalLogin.SetCallback((res)=>{
                Debug.Log(string.Format("SessionId: {0}", res.SessionId));
                PackageReqHead.SessionId = res.SessionId;
            //PackageReqHead.UserId = res.UserData.UserId;
                _loginData.Sid = res.SessionId;
                SocketClient.CreateHeartbeatTimer();

                //IOHelper.SaveToJson<LoginData>(_loginData, URL.ASSETBUNDLE_LOCAL_URL);
                IOHelper.SaveToJson<LoginData>(_loginData, URL.RELATIVE_STREAMINGASSETS_URL);
                });
        _loginUnit = normalLogin;
    }

    private void SetLogin()
    {
        if(_loginData != null)
        {
            InputUsername.value = _loginData.Username;
            InputPassword.value = _loginData.Password;
        }
    }

    private void RegisterBtnEvent()
    {
        Debug.Log("Test");
        EventDelegate.Add(this.BtnLogin.onClick, ()=>{
                this.Login();
                });
    }

    private void Login()
    {
        _loginData.Username = InputUsername.value;
        _loginData.Password = InputPassword.value;
        _loginUnit.SetLoginData(_loginData);
        _loginUnit.Login();
    }
}
