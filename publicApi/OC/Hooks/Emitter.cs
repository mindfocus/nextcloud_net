using System;
using System.Collections.Generic;
using System.Text;

namespace OC.Hooks
{
    /**
     * Class Emitter
     *
     * interface for all classes that are able to emit events
     *
     * @package OC\Hooks
     */
    public interface Emitter
    {
        /**
         * @param string $scope
         * @param string $method
         * @param callable $callback
         * @return void
         */
        void listen(string scope, string method, Action callback);

        /**
         * @param string $scope optional
         * @param string $method optional
         * @param callable $callback optional
         * @return void
         */
        void removeListener(string scope = null, string method = null, Action callback = null);
    }

}
