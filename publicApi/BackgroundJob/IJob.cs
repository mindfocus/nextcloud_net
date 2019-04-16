using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.BackgroundJob
{
    interface IJob : Quartz.IJob
    {
        /**
	 * Run the background job with the registered argument
	 *
	 * @param \OCP\BackgroundJob\IJobList $jobList The job list that manages the state of this job
	 * @param ILogger|null $logger
	 * @since 7.0.0
	 */
        void execute(IJobList jobList, ILogger logger = null);

        /**
         * @param int $id
         * @since 7.0.0
         */
        void setId(int id);

        /**
         * @param int $lastRun
         * @since 7.0.0
         */
        void setLastRun(int lastRun);

        /**
         * @param mixed $argument
         * @since 7.0.0
         */
        void setArgument(object argument);

        /**
         * Get the id of the background job
         * This id is determined by the job list when a job is added to the list
         *
         * @return int
         * @since 7.0.0
         */
        int getId();

        /**
         * Get the last time this job was run as unix timestamp
         *
         * @return int
         * @since 7.0.0
         */
        int getLastRun();

        /**
         * Get the argument associated with the background job
         * This is the argument that will be passed to the background job
         *
         * @return mixed
         * @since 7.0.0
         */
        object getArgument();
    }
}
