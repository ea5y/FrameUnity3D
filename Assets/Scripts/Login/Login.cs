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
        protected LoginData loginData;
        protected Action<LoginDataRes> loginCallback;


        public abstract void Register();
        public abstract void Login();
        public virtual void SetLoginData(LoginData loginData)
        {
            this.loginData = loginData;
        }

        public virtual void SetLoginCallback(Action<LoginDataRes> callback)
        {
            this.loginCallback = callback;
        }
    }

    public class NormalLogin : LoginUnit
    {
        public NormalLogin()
        {
        }

        public override void Register()
        {
        }

        public override void Login()
        {
            Net.Net.Login(this.loginData.Username, 
                    this.loginData.Password,
                    this.loginCallback);
        }
    }
}
