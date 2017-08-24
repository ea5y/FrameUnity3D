using System.Collections.Generic;

namespace Easy.Unity
{
    public class ErrorCode
    {
        public static Dictionary<int, string> Dic = new Dictionary<int, string>()
        {
            { 0, "OK" },
            { 100, "Error" },
            { 101, "SignError" },
            { 102, "NoHandle" },
            { 103, "PassworkError" },
            { 105, "NoToken" },
            { 106, "TokenExpired" },
            { 107, "Timeout" },
            { 108, "ParseError" }
        };
    }
}
