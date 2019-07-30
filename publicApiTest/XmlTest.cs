using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CommonTypes;
using NUnit.Framework;

namespace publicApiTest
{
    public class XmlTest
    {
        [Test]
        public void xmlReadTest()
        {
            var fileName = "/home/focus/server/apps/dav/appinfo/info.xml";
//            var xElement = XElement.Load(fileName);
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlSerializer serializer = 
                new XmlSerializer(typeof(AppInfo));
            var appinfo = (AppInfo)serializer.Deserialize(fs);
            Console.Write(appinfo.Author);
        }
    }
}