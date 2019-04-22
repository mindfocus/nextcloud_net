using System;
using System.Collections.Generic;
using System.Text;
using OCP.Encryption;

namespace OC.Encryption
{
    public class Manager : IManager
    {
        public string getDefaultEncryptionModuleId()
        {
            throw new NotImplementedException();
        }

        public IEncryptionModule getEncryptionModule(string moduleId = "")
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> getEncryptionModules()
        {
            throw new NotImplementedException();
        }

        public bool isEnabled()
        {
            throw new NotImplementedException();
        }

        public void registerEncryptionModule(string id, string displayName, Action callback)
        {
            throw new NotImplementedException();
        }

        public string setDefaultEncryptionModule(string moduleId)
        {
            throw new NotImplementedException();
        }

        public void unregisterEncryptionModule(string moduleId)
        {
            throw new NotImplementedException();
        }
    }
}
