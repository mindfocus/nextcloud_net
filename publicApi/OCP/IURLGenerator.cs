using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
/**
 * Class to generate URLs
 * @since 6.0.0
 */
public interface IURLGenerator {
	/**
	 * Returns the URL for a route
	 * @param string routeName the name of the route
	 * @param array arguments an array with arguments which will be filled into the url
	 * @return string the url
	 * @since 6.0.0
	 */
	string linkToRoute(string routeName, IDictionary<string,string> arguments);

	/**
	 * Returns the absolute URL for a route
	 * @param string routeName the name of the route
	 * @param array arguments an array with arguments which will be filled into the url
	 * @return string the absolute url
	 * @since 8.0.0
	 */
	string linkToRouteAbsolute(string routeName, IDictionary<string,object> arguments);

	/**
	 * @param string routeName
	 * @param array arguments
	 * @return string
	 * @since 15.0.0
	 */
	string linkToOCSRouteAbsolute(string routeName, IList<string> arguments);

	/**
	 * Returns an URL for an image or file
	 * @param string appName the name of the app
	 * @param string file the name of the file
	 * @param array args array with param=>value, will be appended to the returned url
	 *    The value of args will be urlencoded
	 * @return string the url
	 * @since 6.0.0
	 */
	string linkTo(string appName, string file, IDictionary<string,object> args);

	/**
	 * Returns the link to an image, like linkTo but only with prepending img/
	 * @param string appName the name of the app
	 * @param string file the name of the file
	 * @return string the url
	 * @since 6.0.0
	 */
	string imagePath(string appName, string file);


	/**
	 * Makes an URL absolute
	 * @param string url the url in the ownCloud host
	 * @return string the absolute version of the url
	 * @since 6.0.0
	 */
	string getAbsoluteURL(string url) ;

	/**
	 * @param string key
	 * @return string url to the online documentation
	 * @since 8.0.0
	 */
	string linkToDocs(string key);

	/**
	 * @return string base url of the current request
	 * @since 13.0.0
	 */
	string getBaseUrl();
}
}