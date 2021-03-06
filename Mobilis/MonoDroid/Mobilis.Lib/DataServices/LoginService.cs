using System.Collections.Generic;
using Mobilis.Lib.Util;
using System;

namespace Mobilis.Lib.DataServices
{
    public class LoginService : RestService<string>
    {
        public void getToken(string source,string name, string password,ResultCallback<IEnumerable<string>> callback)
        {
            try
            {
                string content = JSON.generateLoginObject(name, password);
                Post(source, null, content, CONTENT_TYPE_JSON, callback);
            }
            catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine("Erro de cnxao");
            }
        }

        /*
        public override IEnumerable<string> parseJSON(string content,int method)
        {
            return JSON.parseToken(content);
        }
        */
        public override IEnumerable<string> parseJSON(System.Net.WebResponse content, int method)
        {
            return JSON.parseToken(HttpUtils.WebResponseToString(content));
        }
    }
}