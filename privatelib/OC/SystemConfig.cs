using System.Collections;
using System.Collections.Generic;

namespace OC
{
    /**
     * Class which provides access to the system config values stored in config.php
     * Internal class for bootstrap only.
     * fixes cyclic DI: AllConfig needs AppConfig needs Database needs AllConfig
     */
    public class SystemConfig
    {
        /** @var array */
        protected IDictionary<string, object> sensitiveValues = new Dictionary<string, object>()
        {
            {"instanceid", true},
            {"datadirectory", true},
            {"dbname", true},
            {"dbhost", true},
            {"dbpassword", true},
            {"dbuser", true},
            {"mail_from_address", true},
            {"mail_domain", true},
            {"mail_smtphost", true},
            {"mail_smtpname", true},
            {"mail_smtppassword", true},
            {"passwordsalt", true},
            {"secret", true},
            {"updater.secret", true},
            {"trusted_proxies", true},
            {"proxyuserpwd", true},
            {
                "log.condition", new Dictionary<string, object>()
                {
                    {"shared_secret", true},
                }
            },
            {"license-key", true},
            {
                "redis", new Dictionary<string, object>()
                {
                    {"host", true},
                    {"password", true},
                }
            },
            {
                "objectstore", new Dictionary<string, object>()
                {
                    {
                        "arguments", new Dictionary<string, object>
                        {
                            {"password", true},
                            {
                                "options", new Dictionary<string, object>
                                {
                                    {
                                        "credentials", new Dictionary<string, object>
                                        {
                                            {"key", true},
                                            {"secret", true},
                                        }
                                    }
                                }
                            }
                        }
                    },
                }
            }
        };

        /** @var Config */
        private Config config;

        public SystemConfig(Config config)
        {
            this.config = config;
        }

        /**
         * Lists all available config keys
         * @return array an array of key names
         */
        public IList<string> getKeys()
        {
            return this.config.getKeys();
        }

        /**
         * Sets a new system wide value
         *
         * @param string key the key of the value, under which will be saved
         * @param mixed value the value that should be stored
         */
        public void setValue(string key, object value)
        {
            this.config.setValue(key, value);
        }

        /**
         * Sets and deletes values and writes the config.php
         *
         * @param array configs Associative array with `key , value` pairs
         *                       If value is null, the config key will be deleted
         */
        public void setValues(IDictionary<string, object>  configs)
        {
            this.config.setValues(configs);
        }

        /**
         * Looks up a system wide defined value
         *
         * @param string key the key of the value, under which it was saved
         * @param mixed default the default value to be returned if the value isn"t set
         * @return mixed the value or default
         */
        public object getValue(string key, object @default = default)
        {
            return this.config.getValue(key, @default);
        }

        /**
         * Looks up a system wide defined value and filters out sensitive data
         *
         * @param string key the key of the value, under which it was saved
         * @param mixed default the default value to be returned if the value isn"t set
         * @return mixed the value or default
         */
        public object getFilteredValue(string key, object @default = default)
        {
            var value = this.getValue(key, default);

            if (this.sensitiveValues.ContainsKey(key))
            {
                value = this.removeSensitiveValue(this.sensitiveValues[key], value);
            }

            return value;
        }

        /**
         * Delete a system wide defined value
         *
         * @param string key the key of the value, under which it was saved
         */
        public void deleteValue(string key)
        {
            this.config.deleteKey(key);
        }

        /**
         * @param bool|array keysToRemove
         * @param mixed value
         * @return mixed
         */
        protected object removeSensitiveValue( IDictionary<string, object> keysToRemove, object value)
        {
            if (keysToRemove == true)
            {
                return IConfig::SENSITIVE_VALUE;
            }

            if (value is IDictionary<string,object> valueList)
            {
                foreach (var toRemove in keysToRemove) {
                    if ( valueList.ContainsKey(toRemove.Key))
                    {
                        valueList[toRemove.Key] = this.removeSensitiveValue(toRemove.Value, valueList[toRemove.Key]);
                    }
                }
            }

            return value;
        }
    }
}