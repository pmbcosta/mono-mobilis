using Mobilis.Lib.Model;
using System.Collections.Generic;
using Mobilis.Lib.Util;

namespace Mobilis.Lib.DataServices
{
    public class ClassService : RestService<Class>
    {
        public void getClasses(string source,string token,ResultCallback<IEnumerable<Class>> callback) 
        {
            Get(source, token, callback);
        }

        public override IEnumerable<Class> parseJSON(string content)
        {
            return JSON.parseClasses(content);
        }
    }
}