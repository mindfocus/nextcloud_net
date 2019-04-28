using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using ext;
using model;
using OCP;

namespace OC
{
/**
 * This class provides an easy way for apps to store config values in the
 * database.
 */
class AppConfig : IAppConfig {

	/** @var array[] */
	protected IDictionary<string,IList<string>> sensitiveValues = new Dictionary<string, IList<string>>()
		{
			{"spreed", new List<string>()
			{
				"turn_server_secret"
			}},
			{"user_ldap", new List<string>()
			{
				"ldap_agent_password"
			}},
		};

	/** @var \OCP\IDBConnection */
	protected IDBConnection conn;

	/** @var array[] */
	private IList<Tuple<string,string,string>>  cache = new List<Tuple<string, string, string>>();

	/** @var bool */
	private bool configLoaded = false;

	/**
	 * @param IDBConnection conn
	 */
	public AppConfig(IDBConnection conn) {
		this.conn = conn;
		this.configLoaded = false;
	}

	/**
	 * @param string app
	 * @return array
	 */
	private IDictionary<string,string> getAppValues(string app) {
		this.loadConfigValues();
		var result = (from tuple in this.cache where tuple.Item1 == app select tuple).ToDictionary(k => k.Item2, v => v.Item3);
		if (result.IsNotEmpty())
		{
			return result;
		}
		return new Dictionary<string, string>();
	}

	/**
	 * Get all apps using the config
	 *
	 * @return array an array of app ids
	 *
	 * This function returns a list of all apps that have at least one
	 * entry in the appconfig table.
	 */
	public IList<string> getApps() {
		this.loadConfigValues();

		return this.getSortedKeys(this.cache.ToDictionary( o=> o.Item1, o =>o.Item2));
	}

	/**
	 * Get the available keys for an app
	 *
	 * @param string app the app we are looking for
	 * @return array an array of key names
	 *
	 * This function gets all keys of an app. Please note that the values are
	 * not returned.
	 */
	public IList<string> getKeys(string app) {
		this.loadConfigValues();
		return (from tuple in this.cache where tuple.Item1 == app select tuple.Item1).ToList();
	}

	public IList<string> getSortedKeys(IDictionary<string , string> data)
	{
		return data.Keys.ToList();
//		keys = array_keys(data);
//		sort(keys);
//		return keys;
	}

	/**
	 * Gets the config value
	 *
	 * @param string app app
	 * @param string key key
	 * @param string default = null, default value if the key does not exist
	 * @return string the value or default
	 *
	 * This function gets a value from the appconfig table. If the key does
	 * not exist the default value will be returned
	 */
	public string getValue(string app, string key, string @default = null) {
		this.loadConfigValues();

		if (this.hasKey(app, key)) {
			return this.cache[app][key];
		}

		return @default;
	}

	/**
	 * check if a key is set in the appconfig
	 *
	 * @param string app
	 * @param string key
	 * @return bool
	 */
	public bool hasKey(string app, string key) {
		this.loadConfigValues();
		return this.cache[app].ContainsKey(key);
	}

	/**
	 * Sets a value. If the key did not exist before it will be created.
	 *
	 * @param string app app
	 * @param string key key
	 * @param string|float|int value value
	 * @return bool True if the value was inserted or updated, false if the value was the same
	 */
	public bool setValue(string app, string key, object value) {
//		if (!this.hasKey(app, key)) {
//			inserted = (bool) this.conn.insertIfNotExist("*PREFIX*appconfig", [
//				"appid" => app,
//				"configkey" => key,
//				"configvalue" => value,
//			], [
//				"appid",
//				"configkey",
//			]);
//
//			if (inserted) {
//				if (!isset(this.cache[app])) {
//					this.cache[app] = [];
//				}
//
//				this.cache[app][key] = value;
//				return true;
//			}
//		}
//
//		sql = this.conn.getQueryBuilder();
//		sql.update("appconfig")
//			.set("configvalue", sql.createParameter("configvalue"))
//			.where(sql.expr().eq("appid", sql.createParameter("app")))
//			.andWhere(sql.expr().eq("configkey", sql.createParameter("configkey")))
//			.setParameter("configvalue", value)
//			.setParameter("app", app)
//			.setParameter("configkey", key);
//
//		/*
//		 * Only limit to the existing value for non-Oracle DBs:
//		 * http://docs.oracle.com/cd/E11882_01/server.112/e26088/conditions002.htm#i1033286
//		 * > Large objects (LOBs) are not supported in comparison conditions.
//		 */
//		if (!(this.conn instanceof OracleConnection)) {
//			// Only update the value when it is not the same
//			sql.andWhere(sql.expr().neq("configvalue", sql.createParameter("configvalue")))
//				.setParameter("configvalue", value);
//		}
//
//		changedRow = (bool) sql.execute();
//
//		this.cache[app][key] = value;
//
//		return changedRow;
		return false;
	}

	/**
	 * Deletes a key
	 *
	 * @param string app app
	 * @param string key key
	 * @return boolean
	 */
	public bool deleteKey(string app, string key) {
		this.loadConfigValues();
		
		sql = this.conn.getQueryBuilder();
		sql.delete("appconfig")
			.where(sql.expr().eq("appid", sql.createParameter("app")))
			.andWhere(sql.expr().eq("configkey", sql.createParameter("configkey")))
			.setParameter("app", app)
			.setParameter("configkey", key);
		sql.execute();

		unset(this.cache[app][key]);
		return false;
	}

	/**
	 * Remove app from appconfig
	 *
	 * @param string app app
	 * @return boolean
	 *
	 * Removes all keys in appconfig belonging to the app.
	 */
	public bool deleteApp(string app) {
		this.loadConfigValues();
		using (var context = new NCContext())
		{
			context.AppConfigs.RemoveRange(context.AppConfigs.Where(o => o.appId == app));
			context.SaveChanges();
		}
		this.cache.ToList().RemoveAll(o => o.Item1 == app);
		return false;
	}

	/**
	 * get multiple values, either the app or key can be used as wildcard by setting it to false
	 *
	 * @param string|false app
	 * @param string|false key
	 * @return array|false
	 */
	public IList<string> getValues(string app, string key) {
		if (app.IsEmpty() && key.IsEmpty()) {
			return new List<string>();
		}

		if (key.IsEmpty()) {
			return this.getAppValues(app).Values.ToList();
		} else {
			appIds = this.getApps();
			values = array_map(function(appId) use (key) {
				return isset(this.cache[appId][key]) ? this.cache[appId][key] : null;
			}, appIds);
			result = array_combine(appIds, values);

			return array_filter(result);
		}
	}

	/**
	 * get all values of the app or and filters out sensitive data
	 *
	 * @param string app
	 * @return array
	 */
	public IList<string> getFilteredValues(string app) {
		values = this.getValues(app, false);

		if (isset(this.sensitiveValues[app])) {
			foreach (this.sensitiveValues[app] as sensitiveKey) {
				if (isset(values[sensitiveKey])) {
					values[sensitiveKey] = IConfig::SENSITIVE_VALUE;
				}
			}
		}

		return values;
	}

	/**
	 * Load all the app config values
	 */
	protected void loadConfigValues() {
		this.cache.Clear();
		if (this.configLoaded) {
			return;
		}
		using (var context = new NCContext())
		{
			foreach (var appConfig in context.AppConfigs)
			{
				var record =
					new Tuple<string, string, string>(appConfig.appId, appConfig.configKey, appConfig.configValue);
				this.cache.Add(record);
			}
		}

		this.configLoaded = true;
	}
}
}