using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.Activity
{
    /**
     * Interface IConsumer
     *
     * @package OCP\Activity
     * @since 6.0.0
     */
    interface IConsumer
    {
        /**
         * @param IEvent $event
         * @return null
         * @since 6.0.0
         * @since 8.2.0 Replaced the parameters with an IEvent object
         */
        object receive(IEvent eventp);

    }
}
