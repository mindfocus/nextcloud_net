﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OC
{
    /**
     * Class ServerContainer
     *
     * @package OC
     */
    class ServerContainer : SimpleContainer
    {
    /** @var DIContainer[] */
    protected appContainers;

	/** @var string[] */
	protected hasNoAppContainer;

	/** @var string[] */
	protected namespaces;

	/**
	 * ServerContainer constructor.
	 */
	public ServerContainer() : base()
    {
		this->appContainers = [];
		this->namespaces = [];
		this->hasNoAppContainer = [];
    }

    /**
	 * @param string appName
	 * @param string appNamespace
	 */
    public function registerNamespace(string appName, string appNamespace): void {
		// Cut of OCA\ and lowercase
		appNamespace = strtolower(substr(appNamespace, strrpos(appNamespace, '\\') + 1));
		this->namespaces[appNamespace] = appName;

    }

    /**
	 * @param string appName
	 * @param DIContainer container
	 */
    public function registerAppContainer(string appName, DIContainer container): void {
		this->appContainers[strtolower(App::buildAppNamespace(appName, ''))] = container;

    }

    /**
	 * @param string appName
	 * @return DIContainer
	 * @throws QueryException
	 */
    public function getRegisteredAppContainer(string appName): DIContainer {
		if (isset(this->appContainers[strtolower(App::buildAppNamespace(appName, ''))])) {
        return this->appContainers[strtolower(App::buildAppNamespace(appName, ''))];
    }

		throw new QueryException();
}

/**
 * @param string namespace
 * @param string sensitiveNamespace
 * @return DIContainer
 * @throws QueryException
 */
protected function getAppContainer(string namespace, string sensitiveNamespace): DIContainer {
		if (isset(this->appContainers[namespace])) {
			return this->appContainers[namespace];
		}

		if (isset(this->namespaces[namespace])) {
			if (!isset(this->hasNoAppContainer[namespace])) {
				applicationClassName = 'OCA\\' . sensitiveNamespace . '\\AppInfo\\Application';
				if (class_exists(applicationClassName)) {
					new applicationClassName();
					if (isset(this->appContainers[namespace])) {
						return this->appContainers[namespace];
					}
}
				this->hasNoAppContainer[namespace] = true;
			}

			return new DIContainer(this->namespaces[namespace]);
		}
		throw new QueryException();
}

/**
 * @param string name name of the service to query for
 * @return mixed registered service for the given name
 * @throws QueryException if the query could not be resolved
 */
public function query(name)
{
		name = this->sanitizeName(name);

    if (isset(this[name]))
    {
        return this[name];
    }

    // In case the service starts with OCA\ we try to find the service in
    // the apps container first.
    if (strpos(name, 'OCA\\') === 0 && substr_count(name, '\\') >= 2)
    {
			segments = explode('\\', name);
        try
        {
				appContainer = this->getAppContainer(strtolower(segments[1]), segments[1]);
            return appContainer->queryNoFallback(name);
        }
        catch (QueryException e) {
            // Didn't find the service or the respective app container,
            // ignore it and fall back to the core container.
        }
        } else if (strpos(name, 'OC\\Settings\\') === 0 && substr_count(name, '\\') >= 3)
        {
			segments = explode('\\', name);
            try
            {
				appContainer = this->getAppContainer(strtolower(segments[1]), segments[1]);
                return appContainer->queryNoFallback(name);
            }
            catch (QueryException e) {
                // Didn't find the service or the respective app container,
                // ignore it and fall back to the core container.
            }
            }

            return parent::query(name);
        }
    }

}
