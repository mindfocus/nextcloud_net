using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{

    /**
     * @since 16.0.0
     */
    public interface IInitialStateService
    {
        /**
         * Allows an app to provide its initial state to the template system.
         * Use this if you know your initial state sill be used for example if
         * you are in the render function of you controller.
         *
         * @since 16.0.0
         *
         * @param string appName
         * @param string key
         * @param bool|int|float|string|array|\JsonSerializable data
         */
        void provideInitialState(string appName, string key, object data);

	/**
	 * Allows an app to provide its initial state via a lazy method.
	 * This will call the closure when the template is being generated.
	 * Use this if your app is injected into pages. Since then the render method
	 * is not called explicitly. But we do not want to load the state on webdav
	 * requests for example.
	 *
	 * @since 16.0.0
	 *
	 * @param string appName
	 * @param string key
	 * @param Closure closure returns a primitive or an object that implements JsonSerializable
	 */
	void provideLazyInitialState(string appName, string key, Action closure);
}

}
