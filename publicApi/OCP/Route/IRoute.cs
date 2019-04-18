using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Route
{

    /**
     * Interface IRoute
     *
     * @package OCP\Route
     * @since 7.0.0
     */
    public interface IRoute
    {
        /**
         * Specify PATCH as the method to use with this route
         * @return \OCP\Route\IRoute
         * @since 7.0.0
         */
        IRoute patch();

        /**
         * Specify the method when this route is to be used
         *
         * @param string $method HTTP method (uppercase)
         * @return \OCP\Route\IRoute
         * @since 7.0.0
         */
        IRoute method(string method);

        /**
         * The action to execute when this route matches, includes a file like
         * it is called directly
         *
         * @param string $file
         * @return void
         * @since 7.0.0
         */
        void actionInclude(string file);

        /**
         * Specify GET as the method to use with this route
         * @return \OCP\Route\IRoute
         * @since 7.0.0
         */
        IRoute get();

        /**
         * Specify POST as the method to use with this route
         * @return \OCP\Route\IRoute
         * @since 7.0.0
         */
        IRoute post();

        /**
         * Specify DELETE as the method to use with this route
         * @return \OCP\Route\IRoute
         * @since 7.0.0
         */
        IRoute delete();

        /**
         * The action to execute when this route matches
         *
         * @param string|callable $class the class or a callable
         * @param string $function the function to use with the class
         * @return \OCP\Route\IRoute
         *
         * This function is called with $class set to a callable or
         * to the class with $function
         * @since 7.0.0
         */
        void action(string classp, string function = null);

	/**
	 * Defaults to use for this route
	 *
	 * @param array $defaults The defaults
	 * @return \OCP\Route\IRoute
	 * @since 7.0.0
	 */
	IRoute defaults(IList<string> defaults);

        /**
         * Requirements for this route
         *
         * @param array $requirements The requirements
         * @return \OCP\Route\IRoute
         * @since 7.0.0
         */
        IRoute requirements(IList<string> requirements);

        /**
         * Specify PUT as the method to use with this route
         * @return \OCP\Route\IRoute
         * @since 7.0.0
         */
        IRoute put();
    }

}
