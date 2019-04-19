using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Diagnostics
{
    /**
     * Interface IQuery
     *
     * @package OCP\Diagnostics
     * @since 8.0.0
     */
    public interface IQuery
    {
        /**
         * @return string
         * @since 8.0.0
         */
        string getSql();

        /**
         * @return array
         * @since 8.0.0
         */
        PhpArray getParams();

        /**
         * @return float
         * @since 8.0.0
         */
        float getDuration();

        /**
         * @return float
         * @since 11.0.0
         */
        float getStartTime();

        /**
         * @return array
         * @since 11.0.0
         */
        PhpArray getStacktrace();
        /**
         * @return array
         * @since 12.0.0
         */
        PhpArray getStart();
    }

}
