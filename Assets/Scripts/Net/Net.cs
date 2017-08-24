using System;

namespace Easy.Unity
{
    public class Net
    {
        public static void Login(Action<BaseResData> callback)
        {
            var data = new RegisterData() { Username = "easy9", Password = "443322" };
            var bytes = PackageFactory.Pack(1002, data, callback);
            SocketClient.Send(bytes);
        }
    }
}
