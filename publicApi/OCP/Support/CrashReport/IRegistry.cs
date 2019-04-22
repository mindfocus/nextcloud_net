using System;
using System.Collections.Generic;

namespace OCP.Support.CrashReport
{
    /**
     * @since 13.0.0
     */
    public interface IRegistry
    {

        /**
         * Register a reporter instance
         *
         * @since 13.0.0
         * @param IReporter reporter
         */
        void register(IReporter reporter);

        /**
         * Delegate breadcrumb collection to all registered reporters
         *
         * @param string message
         * @param string category
         * @param array context
         *
         * @since 15.0.0
         */
        void delegateBreadcrumb(string message, string category, IList<string> context = []);

        /**
         * Delegate crash reporting to all registered reporters
         *
         * @since 13.0.0
         * @param Exception|Throwable exception
         * @param array context
         */
        void delegateReport(System.Exception exception, IList<string> context);
    }

}
