using Pchp.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace OC
{
    /**
     * This class is responsible for reading and writing config.php, the very basic
     * configuration file of Nextcloud.
     */
    public class Config
    {

        const string ENV_PREFIX = "NC_";

        /** @var array Associative array (key => value) */
        protected IDictionary<string, object> cache = new Dictionary<string, object>();

        /** @var string */
        protected string configDir;

        /** @var string */
        protected string configFilePath;

        /** @var string */
        protected string configFileName;

        /**
         * @param string configDir Path to the config dir, needs to end with '/'
         * @param string fileName (Optional) Name of the config file. Defaults to config.php
         */
        Config(string configDir, string fileName = "config.php")
        {
            this.configDir = configDir;
            this.configFileName = fileName;
            this.configFilePath = configDir + "/" + fileName;
            readData();
        }

        /**
         * Lists all available config keys
         *
         * Please note that it does not return the values.
         *
         * @return array an array of key names
         */
        public IList<string> getKeys()
        {
            return this.cache.Keys.ToList();
            //return array_keys(this.cache);
        }

        /**
         * Returns a config value
         *
         * gets its value from an `NC_` prefixed environment variable
         * if it doesn't exist from config.php
         * if this doesn't exist either, it will return the given `default`
         *
         * @param string key key
         * @param mixed default = null default value
         * @return mixed the value or default
         */
        public object getValue(string key, object @default = null)
        {
            var envValue = System.Environment.GetEnvironmentVariable(ENV_PREFIX + key);
            if (envValue != null)
            {
                return envValue;
            }

            if (this.cache.ContainsKey(key))
            {
                return this.cache[key];
            }

            return @default;
        }

        /**
         * Sets and deletes values and writes the config.php
         *
         * @param array configs Associative array with `key => value` pairs
         *                       If value is null, the config key will be deleted
         */
        public void setValues(IDictionary<string, object> configs)
        {
            var needsUpdate = false;
            foreach (var config in configs)
            {
                if (config.Value != null)
                {
                    needsUpdate |= this.set(config.Key, config.Value);
                }
                else
                {
                    needsUpdate |= this.delete(config.Key);
                }
            }

            if (needsUpdate)
            {
                // Write changes
                this.writeData();
            }
        }

        /**
         * Sets the value and writes it to config.php if required
         *
         * @param string key key
         * @param mixed value value
         */
        public void setValue(string key, object value)
        {
            if (this.set(key, value))
            {
                // Write changes
                this.writeData();
            }
        }

        /**
         * This function sets the value
         *
         * @param string key key
         * @param mixed value value
         * @return bool True if the file needs to be updated, false otherwise
         */
        protected bool set(string key, object value)
        {
            if (this.cache.ContainsKey(key) || this.cache[key] != value)
            {
                this.cache[key] = value;
                return true;
            }

            return false;

        }

        /**
         * Removes a key from the config and removes it from config.php if required
         * @param string key
         */
        public void deleteKey(string key)
        {
            if (this.delete(key))
            {
                // Write changes
                this.writeData();
            }
        }

        /**
         * This function removes a key from the config
         *
         * @param string key
         * @return bool True if the file needs to be updated, false otherwise
         */
        protected bool delete(string key)
        {
            return this.cache.Remove(key);
        }

        /**
         * Loads the config file
         *
         * Reads the config file and saves it to the cache
         *
         * @throws \Exception If no lock could be acquired or the config file has not been found
         */
        private void readData()
        {
//		// Default config should always get loaded
//		var configFiles = array(this.configFilePath);
//
//		// Add all files in the config dir ending with the same file name
//		extra = glob(this.configDir.'*.'.this.configFileName);
//            if (is_array(extra))
//            {
//                natsort(extra);
//			configFiles = array_merge(configFiles, extra);
//            }
//
//            // Include file and merge config
//            foreach (configFiles as file) {
//			fileExistsAndIsReadable = file_exists(file) && is_readable(file);
//			filePointer = fileExistsAndIsReadable? fopen(file, 'r') : false;
//                if (file === this.configFilePath &&
//    				filePointer === false) {
//                    // Opening the main config might not be possible, e.g. if the wrong
//                    // permissions are set (likely on a new installation)
//                    continue;
//                }
//
//                // Try to acquire a file lock
//                if (!flock(filePointer, LOCK_SH))
//                {
//                    throw new \Exception(sprintf('Could not acquire a shared lock on the config file %s', file));
//                }
//
//                unset(CONFIG);
//                include file;
//                if (isset(CONFIG) && is_array(CONFIG))
//                {
//				this.cache = array_merge(this.cache, CONFIG);
//                }
//
//                // Close the file pointer and release the lock
//                flock(filePointer, LOCK_UN);
//                fclose(filePointer);
//            }
        }

        /**
         * Writes the config file
         *
         * Saves the config to the config file.
         *
         * @throws HintException If the config file cannot be written to
         * @throws \Exception If no file lock can be acquired
         */
        private void writeData()
        {
//		// Create a php file ...
//		content = "<?php\n";
//		content.= 'CONFIG = ';
//		content.= var_export(this.cache, true);
//		content.= ";\n";
//
//            touch(this.configFilePath);
//		filePointer = fopen(this.configFilePath, 'r+');
//
//            // Prevent others not to read the config
//            chmod(this.configFilePath, 0640);
//
//            // File does not exist, this can happen when doing a fresh install
//            if (!is_resource(filePointer))
//            {
//			// TODO fix this via DI once it is very clear that this doesn't cause side effects due to initialization order
//			// currently this breaks app routes but also could have other side effects especially during setup and exception handling
//			url = \OC::server.getURLGenerator().linkToDocs('admin-dir_permissions');
//                throw new HintException(
//                    "Can't write into config directory!",
//                    'This can usually be fixed by giving the webserver write access to the config directory. See '. url);
//            }
//
//            // Try to acquire a file lock
//            if (!flock(filePointer, LOCK_EX))
//            {
//                throw new \Exception(sprintf('Could not acquire an exclusive lock on the config file %s', this.configFilePath));
//            }
//
//            // Write the config and release the lock
//            ftruncate(filePointer, 0);
//            fwrite(filePointer, content);
//            fflush(filePointer);
//            flock(filePointer, LOCK_UN);
//            fclose(filePointer);
//
//            if (function_exists('opcache_invalidate'))
//            {
//                @opcache_invalidate(this.configFilePath, true);
//            }
//        }
        }


    }
}
