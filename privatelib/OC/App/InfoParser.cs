using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CommonTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OC.App
{
    public class InfoParser
    {
        private readonly OCP.ICache cache;

        public InfoParser(OCP.ICache cache)
        {
            this.cache = cache;
        }
        /**
         * @param string $file the xml file to be loaded
         * @return null|array where null is an indicator for an error
         */
        public AppInfo parse(string file)
        {
	        if (!File.Exists(file))
	        {
		        return null;
	        }

	        if (this.cache != null)
	        {
		        var fileCacheKey = file + File.GetLastWriteTime(file).ToFileTime().ToString();
		        var cachedValue = this.cache.get(fileCacheKey);
		        if (cachedValue != null)
		        {
			        return JsonConvert.DeserializeObject<AppInfo>(cachedValue.ToString());
		        }
	        }

	        AppInfo appInfo = null;
	        using (var fs = new FileStream(file, FileMode.Open))
	        {
		        XmlSerializer serializer = 
			        new XmlSerializer(typeof(AppInfo));
		        appInfo = (AppInfo)serializer.Deserialize(fs);
	        }

            return appInfo;
        }
    }
}