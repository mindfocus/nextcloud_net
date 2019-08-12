using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OC.Hooks
{
    public abstract class BasicEmitter : Emitter
    {

    /**
	 * @var callable[][] listeners
	 */
    protected IDictionary<string,IList<Action<IList<string>>>> listeners = new Dictionary<string, IList<Action<IList<string>>>>();

    /**
	 * @param string scope
	 * @param string method
	 * @param callable callback
	 */
    public void listen(string scope, string method, Action<IList<string>> callback)
    {
		var eventName = scope + "." + method;

		if (!this.listeners.ContainsKey(eventName))
        {
	        this.listeners.Add(eventName, new List<Action<IList<string>>>());
        }

        if (!this.listeners[eventName].Contains(callback))
        {
	        this.listeners[eventName].Add(callback);
        }
    }

    /**
	 * @param string scope optional
	 * @param string method optional
	 * @param callable callback optional
	 */
    public void removeListener(string scope = null, string method = null, Action<IList<string>> callback = null)
    {
		var names = new List<string>();
		var allNames = this.listeners.Keys.ToList();
        if (scope != null && method != null) {
			var name = scope + "." + method;
			if (this.listeners.ContainsKey(name))
			{
				names.Add(name);
			}
        }
        else if(scope != null) {
            foreach (var name in allNames)
            {
	            var parts = name.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); // explode('::', name, 2);
                if (parts[0] == scope)
                {
	                names.Add(name);
                }
            }
        }
        else if(method != null) {
            foreach (var name in allNames) {
	            var parts = name.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); // explode('::', name, 2);
	            if (parts[1] == method)
	            {
		            names.Add(name);
	            }
            }
        } else
        {
			names = allNames;
        }

        foreach (var name in names) {
            if (callback != null)
            {
	            var exist = this.listeners[name].Contains(callback);
                if (exist)
                {
	                this.listeners[name].Remove(callback);
                }
            } else
            {
				this.listeners[name] = new List<Action<IList<string>>>();
            }
        }
    }

    /**
	 * @param string scope
	 * @param string method
	 * @param array arguments optional
	 */
    protected void emit(string scope, string method, IList<string> arguments)
    {
		var eventName = scope + "." + method;
        if (this.listeners.ContainsKey(eventName))
        {
            foreach (var callback in this.listeners[eventName])
            {
	            callback(arguments);
//                call_user_func_array(callback, arguments);
            }
        }
    }
}

}
