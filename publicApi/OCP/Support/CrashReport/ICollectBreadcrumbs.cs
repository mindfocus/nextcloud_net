using System;
namespace OCP.Support.CrashReport
{
    /**
     * @since 15.0.0
     */
    public interface ICollectBreadcrumbs : IReporter
    {

    /**
     * Collect breadcrumbs for crash reports
     *
     * @param string message
     * @param string category
     * @param array context
     *
     * @since 15.0.0
     */
    void collect(string message, string category, IList<string> context);

}

}
