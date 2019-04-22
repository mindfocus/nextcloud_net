using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Encryption.Keys
{
    /**
     * Interface IStorage
     *
     * @package OCP\Encryption\Keys
     * @since 8.1.0
     */
    public interface IStorage
    {

        /**
         * get user specific key
         *
         * @param string uid ID if the user for whom we want the key
         * @param string keyId id of the key
         * @param string encryptionModuleId
         *
         * @return mixed key
         * @since 8.1.0
         */
        object getUserKey(string uid, string keyId, string encryptionModuleId);

        /**
         * get file specific key
         *
         * @param string path path to file
         * @param string keyId id of the key
         * @param string encryptionModuleId
         *
         * @return mixed key
         * @since 8.1.0
         */
        object getFileKey(string path, string keyId, string encryptionModuleId);

        /**
         * get system-wide encryption keys not related to a specific user,
         * e.g something like a key for public link shares
         *
         * @param string keyId id of the key
         * @param string encryptionModuleId
         *
         * @return mixed key
         * @since 8.1.0
         */
        object getSystemUserKey(string keyId, string encryptionModuleId);

        /**
         * set user specific key
         *
         * @param string uid ID if the user for whom we want the key
         * @param string keyId id of the key
         * @param mixed key
         * @param string encryptionModuleId
         * @since 8.1.0
         */
        void setUserKey(string uid, string keyId, object key, string encryptionModuleId);

        /**
         * set file specific key
         *
         * @param string path path to file
         * @param string keyId id of the key
         * @param mixed key
         * @param string encryptionModuleId
         * @since 8.1.0
         */
        void setFileKey(string path, string keyId, object key, string encryptionModuleId);

        /**
         * set system-wide encryption keys not related to a specific user,
         * e.g something like a key for public link shares
         *
         * @param string keyId id of the key
         * @param mixed key
         * @param string encryptionModuleId
         *
         * @return mixed key
         * @since 8.1.0
         */
        object setSystemUserKey(string keyId, object key, string encryptionModuleId);

        /**
         * delete user specific key
         *
         * @param string uid ID if the user for whom we want to delete the key
         * @param string keyId id of the key
         * @param string encryptionModuleId
         *
         * @return boolean False when the key could not be deleted
         * @since 8.1.0
         */
        bool deleteUserKey(string uid, string keyId, string encryptionModuleId);

        /**
         * delete file specific key
         *
         * @param string path path to file
         * @param string keyId id of the key
         * @param string encryptionModuleId
         *
         * @return boolean False when the key could not be deleted
         * @since 8.1.0
         */
        bool deleteFileKey(string path, string keyId, string encryptionModuleId);

        /**
         * delete all file keys for a given file
         *
         * @param string path to the file
         *
         * @return boolean False when the keys could not be deleted
         * @since 8.1.0
         */
        bool deleteAllFileKeys(string path);

        /**
         * delete system-wide encryption keys not related to a specific user,
         * e.g something like a key for public link shares
         *
         * @param string keyId id of the key
         * @param string encryptionModuleId
         *
         * @return boolean False when the key could not be deleted
         * @since 8.1.0
         */
        bool deleteSystemUserKey(string keyId, string encryptionModuleId);

        /**
         * copy keys if a file was renamed
         *
         * @param string source
         * @param string target
         * @return boolean
         * @since 8.1.0
         */
        bool renameKeys(string source, string target);

        /**
         * move keys if a file was renamed
         *
         * @param string source
         * @param string target
         * @return boolean
         * @since 8.1.0
         */
        bool copyKeys(string source, string target);

        /**
         * backup keys of a given encryption module
         *
         * @param string encryptionModuleId
         * @param string purpose
         * @param string uid
         * @return bool
         * @since 12.0.0
         */
        bool backupUserKeys(string encryptionModuleId, string purpose, string uid);
    }

}
