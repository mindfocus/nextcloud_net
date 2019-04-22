using System;
using System.Collections.Generic;

namespace OCP.Support.CrashReport
{
    /**
     * @since 13.0.0
     */
    public interface IReporter
    {

        /**
         * Report an (unhandled) exception
         *
         * @since 13.0.0
         * @param Exception|Throwable exception
         * @param array context
         */
        void report(System.Exception exception, IList<string> context);
    }

}
