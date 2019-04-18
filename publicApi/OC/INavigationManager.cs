using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi
{
    /**
     * Manages the ownCloud navigation
     * @since 6.0.0
     */
    interface INavigationManager
    {

        /**
         * Navigation entries of the app navigation
         * @since 16.0.0
         */
        //const TYPE_APPS = 'link';

        /**
         * Navigation entries of the settings navigation
         * @since 16.0.0
         */
        //const TYPE_SETTINGS = 'settings';

        /**
         * Navigation entries for public page footer navigation
         * @since 16.0.0
         */
        //const TYPE_GUEST = 'guest';

        /**
         * Creates a new navigation entry
         *
         * @param array|\Closure $entry Array containing: id, name, order, icon and href key
         *					The use of a closure is preferred, because it will avoid
         * 					loading the routing of your app, unless required.
         * @return void
         * @since 6.0.0
         */
        void add(Action entry);
        void add(IList<Action> entry);

        /**
         * Sets the current navigation entry of the currently running app
         * @param string $appId id of the app entry to activate (from added $entry)
         * @return void
         * @since 6.0.0
         */
        void setActiveEntry(string appId);

        /**
         * Get a list of navigation entries
         *
         * @param string $type type of the navigation entries
         * @return array
         * @since 14.0.0
         */
        //IList<string> getAll(string type = self::TYPE_APPS);
        IList<string> getAll(string type );
    }

}
