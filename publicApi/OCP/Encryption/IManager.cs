using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Encryption
{
    /**
 * This class provides access to files encryption apps.
 *
 * @since 8.1.0
 */
public interface IManager {

	/**
	 * Check if encryption is available (at least one encryption module needs to be enabled)
	 *
	 * @return bool true if enabled, false if not
	 * @since 8.1.0
	 */
	bool isEnabled();

	/**
	 * Registers an callback function which must return an encryption module instance
	 *
	 * @param string $id
	 * @param string $displayName
	 * @param callable $callback
	 * @throws ModuleAlreadyExistsException
	 * @since 8.1.0
	 */
	void registerEncryptionModule(string id, string displayName, Action callback);

	/**
	 * Unregisters an encryption module
	 *
	 * @param string $moduleId
	 * @since 8.1.0
	 */
	void unregisterEncryptionModule(string moduleId);

	/**
	 * get a list of all encryption modules
	 *
	 * @return array [id => ['id' => $id, 'displayName' => $displayName, 'callback' => callback]]
	 * @since 8.1.0
	 */
	IDictionary<string,object> getEncryptionModules();


	/**
	 * get a specific encryption module
	 *
	 * @param string $moduleId Empty to get the default module
	 * @return IEncryptionModule
	 * @throws ModuleDoesNotExistsException
	 * @since 8.1.0
	 */
	IEncryptionModule getEncryptionModule(string moduleId = "");

	/**
	 * get default encryption module Id
	 *
	 * @return string
	 * @since 8.1.0
	 */
	string getDefaultEncryptionModuleId();

	/**
	 * set default encryption module Id
	 *
	 * @param string $moduleId
	 * @return string
	 * @since 8.1.0
	 */
	string setDefaultEncryptionModule(string moduleId);

}

}