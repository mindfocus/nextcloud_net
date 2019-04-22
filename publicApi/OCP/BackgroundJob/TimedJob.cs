using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OCP.AppFramework.Utility;
using Quartz;

namespace OCP.BackgroundJob
{
    /**
     * Simple base class to extend to run periodic background jobs.
     * Call setInterval with your desired interval in seconds from the constructor.
     *
     * @since 15.0.0
     */
    abstract class TimedJob : Job
    {
    /** @var int */
    protected int interval = 0;

        public TimedJob(ITimeFactory time) : base(time)
        {
        }

        /**
         * set the interval for the job
         *
         * @since 15.0.0
         */
        void setInterval(int interval)
    {
            this.interval = interval;
    }

    /**
	 * run the job if the last run is is more than the interval ago
	 *
	 * @param JobList jobList
	 * @param ILogger|null logger
	 *
	 * @since 15.0.0
	 */
    public void execute(IJobList jobList, ILogger logger = null)
    {
        if ((this.time.getTime() - this.lastRun) > this.interval) {
            execute(jobList, logger);
        }
    }
}
}
