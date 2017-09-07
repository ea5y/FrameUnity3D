//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-07 19:35
//================================

using System;
using Easy.FrameUnity.Net;

namespace Easy.FrameUnity.Login
{
    public abstract class LoginUnit
    {
        public abstract void Register();
        public abstract void Login();
        public abstract void SetLoginData(LoginData loginData);
    }

    public class NormalLogin : LoginUnit
    {
        private LoginData _loginData;
        private Action<LoginDataRes> _loginCallback;

        public NormalLogin(LoginData loginData)
        {
            _loginData = loginData;
        }

        public override void Register()
        {
        }

        public override void Login()
        {
            Net.Net.Login(_loginData.Username, 
                    _loginData.Password,
                    _loginCallback);
        }

        public override void SetLoginData(LoginData loginData)
        {
            _loginData = loginData;
        }

        public void SetCallback(Action<LoginDataRes> callback)
        {
            _loginCallback = callback;
        }
    }
}

