using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Search
{
    /**
     * Provides a template for search functionality throughout ownCloud;
     * @since 7.0.0
     */
    public abstract class Provider
    {

        /**
         * @since 8.0.0
         */
        string OPTION_APPS = "apps";

        /**
         * List of options
         * @var array
         * @since 7.0.0
         */
        protected IDictionary<string,object> options;

	/**
	 * Constructor
	 * @param array options as key => value
	 * @since 7.0.0 - default value for options was added in 8.0.0
	 */
	public Provider(IDictionary<string, object>  options)
        {
            this.options = options;
        }

        /**
         * get a value from the options array or null
         * @param string key
         * @return mixed
         * @since 8.0.0
         */
        public object getOption(string key)
        {
            if(this.options == null || !this.options.ContainsKey(key))
            {
                return null;
            }
            return this.options[key];
        }

        /**
         * checks if the given apps and the apps this provider has results for intersect
         * returns true if the given array is empty (all apps)
         * or if this provider does not have a list of apps it provides results for (legacy search providers)
         * or if the two above arrays have elements in common (intersect)
         * @param string[] apps
         * @return bool
         * @since 8.0.0
         */
        public bool providesResultsFor(IList<string> apps )
        {
            var forApps = this.getOption(this.OPTION_APPS);
            return forApps == null || apps.Contains((string)forApps);
            //return empty(apps) || empty(forApps) || array_intersect(forApps, apps);
        }

        /**
         * Search for query
         * @param string query
         * @return array An array of OCP\Search\Result's
         * @since 7.0.0
         */
        abstract public IList<Result> search(string query);
    }

}
