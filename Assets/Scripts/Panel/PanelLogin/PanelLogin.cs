//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-07 13:57
//================================

using UnityEngine;
using Easy.FrameUnity.Net;
using Easy.FrameUnity.Login;
using Easy.FrameUnity.Model;
using Easy.FrameUnity.Util;

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
        _loginData = IOHelperUtil.ReadFromJson<LoginData>(URL.PERSISTENTDATA_URL);
        PackageReqHead.SessionId = _loginData.Sid;
    }

    private void CreateLoginUnit()
    {
        _loginUnit = new NormalLogin();
        _loginUnit.SetLoginData(_loginData);
        _loginUnit.SetLoginCallback((res)=>{
                Debug.Log(string.Format("SessionId: {0}", res.SessionId));
                PackageReqHead.SessionId = res.SessionId;
                _loginData.Sid = res.SessionId;
                IOHelperUtil.SaveToJson<LoginData>(_loginData, URL.PERSISTENTDATA_URL);

                Player.Inst.UserData = res.UserData;

                SocketClient.CreateHeartbeatTimer();
            //Enter Game
            //ScenesManager.Inst.EnterScene(SceneName.E_SceneGame_1);
            ScenesManager.Inst.EnterLoadingScene(SceneName.E_SceneGame_1);
                //PlayerManager.Inst.PersonalPlayerSpawn();
                });
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
        this.UpdateLoginData();
        _loginUnit.Login();
    }

    private void UpdateLoginData()
    {
        _loginData.Username = InputUsername.value;
        _loginData.Password = InputPassword.value;
        _loginUnit.SetLoginData(_loginData);
    }
}
