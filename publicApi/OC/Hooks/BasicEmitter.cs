﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OC.Hooks
{
    abstract class BasicEmitter : Emitter
    {

    /**
	 * @var callable[][] $listeners
	 */
    protected IList<IList<Action>> listeners;

    /**
	 * @param string $scope
	 * @param string $method
	 * @param callable $callback
	 */
    public void listen(string scope, string method, Action callback)
    {
		var eventName = scope + "::" + method;
        if (!isset($this.listeners[$eventName]))
        {
			$this.listeners[$eventName] = array();
        }
        if (array_search($callback, $this.listeners[$eventName], true) === false)
        {
			$this.listeners[$eventName][] = $callback;
        }
    }

    /**
	 * @param string $scope optional
	 * @param string $method optional
	 * @param callable $callback optional
	 */
    public function removeListener($scope = null, $method = null, callable $callback = null)
    {
		$names = array();
		$allNames = array_keys($this.listeners);
        if ($scope and $method) {
			$name = $scope. '::'. $method;
            if (isset($this.listeners[$name]))
            {
				$names[] = $name;
            }
        }
        elseif($scope) {
            foreach ($allNames as $name) {
				$parts = explode('::', $name, 2);
                if ($parts[0] == $scope) {
					$names[] = $name;
                }
            }
        }
        elseif($method) {
            foreach ($allNames as $name) {
				$parts = explode('::', $name, 2);
                if ($parts[1] == $method) {
					$names[] = $name;
                }
            }
        } else
        {
			$names = $allNames;
        }

        foreach ($names as $name) {
            if ($callback) {
				$index = array_search($callback, $this.listeners[$name], true);
                if ($index !== false) {
                    unset($this.listeners[$name][$index]);
                }
            } else
            {
				$this.listeners[$name] = array();
            }
        }
    }

    /**
	 * @param string $scope
	 * @param string $method
	 * @param array $arguments optional
	 */
    protected void emit(string scope, string method, IList<string> arguments)
    {
		$eventName = $scope. '::'. $method;
        if (isset($this.listeners[$eventName]))
        {
            foreach ($this.listeners[$eventName] as $callback) {
                call_user_func_array($callback, $arguments);
            }
        }
    }
}

}
