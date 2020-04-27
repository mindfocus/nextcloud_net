namespace OC
{
    public class SystemConfig
    {
        /** @var array */
	protected sensitiveValues = [
		"instanceid" => true,
		"datadirectory" => true,
		"dbname" => true,
		"dbhost" => true,
		"dbpassword" => true,
		"dbuser" => true,
		"mail_from_address" => true,
		"mail_domain" => true,
		"mail_smtphost" => true,
		"mail_smtpname" => true,
		"mail_smtppassword" => true,
		"passwordsalt" => true,
		"secret" => true,
		"updater.secret" => true,
		"trusted_proxies" => true,
		"proxyuserpwd" => true,
		"log.condition" => [
			"shared_secret" => true,
		],
		"license-key" => true,
		"redis" => [
			"host" => true,
			"password" => true,
		],
		"objectstore" => [
			"arguments" => [
				// Legacy Swift (https://github.com/nextcloud/server/pull/17696#discussion_r341302207)
				"options" => [
					"credentials" => [
						"key" => true,
						"secret" => true,
					]
				],
				// S3
				"key" => true,
				"secret" => true,
				// Swift v2
				"username" => true,
				"password" => true,
				// Swift v3
				"user" => [
					"name" => true,
					"password" => true,
				],
			],
		],
	];

	/** @var Config */
	private config;

	public SystemConfig(Config config) {
		this.config = config;
	}

	/**
	 * Lists all available config keys
	 * @return array an array of key names
	 */
	public function getKeys() {
		return this.config.getKeys();
	}

	/**
	 * Sets a new system wide value
	 *
	 * @param string key the key of the value, under which will be saved
	 * @param mixed value the value that should be stored
	 */
	public function setValue(key, value) {
		this.config.setValue(key, value);
	}

	/**
	 * Sets and deletes values and writes the config.php
	 *
	 * @param array configs Associative array with `key => value` pairs
	 *                       If value is null, the config key will be deleted
	 */
	public function setValues(array configs) {
		this.config.setValues(configs);
	}

	/**
	 * Looks up a system wide defined value
	 *
	 * @param string key the key of the value, under which it was saved
	 * @param mixed default the default value to be returned if the value isn"t set
	 * @return mixed the value or default
	 */
	public function getValue(key, default = "") {
		return this.config.getValue(key, default);
	}

	/**
	 * Looks up a system wide defined value and filters out sensitive data
	 *
	 * @param string key the key of the value, under which it was saved
	 * @param mixed default the default value to be returned if the value isn"t set
	 * @return mixed the value or default
	 */
	public function getFilteredValue(key, default = "") {
		value = this.getValue(key, default);

		if (isset(this.sensitiveValues[key])) {
			value = this.removeSensitiveValue(this.sensitiveValues[key], value);
		}

		return value;
	}

	/**
	 * Delete a system wide defined value
	 *
	 * @param string key the key of the value, under which it was saved
	 */
	public function deleteValue(key) {
		this.config.deleteKey(key);
	}

	/**
	 * @param bool|array keysToRemove
	 * @param mixed value
	 * @return mixed
	 */
	protected object removeSensitiveValue(keysToRemove, value) {
		if (keysToRemove === true) {
			return IConfig::SENSITIVE_VALUE;
		}

		if (is_array(value)) {
			foreach (keysToRemove as keyToRemove => valueToRemove) {
				if (isset(value[keyToRemove])) {
					value[keyToRemove] = this.removeSensitiveValue(valueToRemove, value[keyToRemove]);
				}
			}
		}

		return value;
	}
    }
}