using System;
using System.Collections.Generic;
using System.Text;
using publicApi.AppFramework.Utility;

namespace OCP.BackgroundJob
{
    /**
     * Simple base class for a one time background job
     *
     * @since 15.0.0
     */
    abstract class QueuedJob : Job
    {
        public QueuedJob(ITimeFactory time) : base(time)
        {
        }

        /**
         * run the job, then remove it from the joblist
         *
         * @param IJobList $jobList
         * @param ILogger|null $logger
         *
         * @since 15.0.0
         */
        public void execute(IJobList jobList, ILogger logger = null)
        {
            jobList.remove(this, this.argument);
            execute(jobList, logger);
        }
    }

}
