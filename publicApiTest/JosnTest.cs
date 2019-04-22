using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace publicApiTest
{
    public class JosnTest
    {
        [Test]
        public void jsontest()
        {
            var str = Properties.Resources.accountInfo;
            var jo = JObject.Parse(str);
            var needmodify = new Dictionary<string, string>();
            foreach (var child in jo)
            {
                var key = child.Key;
                var value = child.Value;
                var dic = ((JObject)value).Properties().ToDictionary(p => p.Name , p=>p.Value);
                if(dic.ContainsKey("verified"))
                {
                    dic["verified"] = "1";
                }
                var dicstr = JsonConvert.SerializeObject(dic, Formatting.Indented);
                jo[value.Path] = JObject.Parse(dicstr);
            }
            var displayname = jo["displayname"];
        }
    }
}
