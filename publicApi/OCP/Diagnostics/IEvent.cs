using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Diagnostics
{
    /**
     * Interface IEvent
     *
     * @package OCP\Diagnostics
     * @since 8.0.0
     */
    public interface IEvent
    {
        /**
         * @return string
         * @since 8.0.0
         */
        string getId();

        /**
         * @return string
         * @since 8.0.0
         */
        string getDescription();

        /**
         * @return float
         * @since 8.0.0
         */
        float getStart();

        /**
         * @return float
         * @since 8.0.0
         */
        float getEnd();

        /**
         * @return float
         * @since 8.0.0
         */
        float getDuration();
    }

}
