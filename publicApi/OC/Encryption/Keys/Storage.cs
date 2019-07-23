using OC.Files;

namespace OC.Encryption.Keys
{
    public class Storage : OCP.Encryption.Keys.IStorage
    {
        public Storage(View view , Util util)
        {
            
        }
        public object getUserKey(string uid, string keyId, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public object getFileKey(string path, string keyId, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public object getSystemUserKey(string keyId, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public void setUserKey(string uid, string keyId, object key, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public void setFileKey(string path, string keyId, object key, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public object setSystemUserKey(string keyId, object key, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public bool deleteUserKey(string uid, string keyId, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public bool deleteFileKey(string path, string keyId, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public bool deleteAllFileKeys(string path)
        {
            throw new System.NotImplementedException();
        }

        public bool deleteSystemUserKey(string keyId, string encryptionModuleId)
        {
            throw new System.NotImplementedException();
        }

        public bool renameKeys(string source, string target)
        {
            throw new System.NotImplementedException();
        }

        public bool copyKeys(string source, string target)
        {
            throw new System.NotImplementedException();
        }

        public bool backupUserKeys(string encryptionModuleId, string purpose, string uid)
        {
            throw new System.NotImplementedException();
        }
    }
}