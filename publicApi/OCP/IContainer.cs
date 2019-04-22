using System;
using System.Collections;
using System.Collections.Generic;
namespace OCP
{
/**
 * Class IContainer
 *
 * IContainer is the basic interface to be used for any internal dependency injection mechanism
 *
 * @package OCP
 * @since 6.0.0
 */
public interface IContainer {

	/**
	 * If a parameter is not registered in the container try to instantiate it
	 * by using reflection to find out how to build the class
	 * @param string name the class name to resolve
	 * @return \stdClass
	 * @since 8.2.0
	 * @throws QueryException if the class could not be found or instantiated
	 */
	 Type resolve(string name);

	/**
	 * Look up a service for a given name in the container.
	 *
	 * @param string name
	 * @return mixed
	 * @throws QueryException if the query could not be resolved
	 * @since 6.0.0
	 */
	 object query(string name);

	/**
	 * A value is stored in the container with it's corresponding name
	 *
	 * @param string name
	 * @param mixed value
	 * @return void
	 * @since 6.0.0
	 */
	public void registerParameter(string name, object value);

	/**
	 * A service is registered in the container where a closure is passed in which will actually
	 * create the service on demand.
	 * In case the parameter shared is set to true (the default usage) the once created service will remain in
	 * memory and be reused on subsequent calls.
	 * In case the parameter is false the service will be recreated on every call.
	 *
	 * @param string name
	 * @param \Closure closure
	 * @param bool shared
	 * @return void
	 * @since 6.0.0
	 */
	 void registerService(string name, Action closure, bool shared = true);

	/**
	 * Shortcut for returning a service from a service under a different key,
	 * e.g. to tell the container to return a class when queried for an
	 * interface
	 * @param string alias the alias that should be registered
	 * @param string target the target that should be resolved instead
	 * @since 8.2.0
	 */
	 void registerAlias(string alias, string target);
}

}
