using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.BackgroundJob
{
    /**
     * Interface IJobList
     *
     * This interface provides functions to register background jobs
     *
     * To create a new background job create a new class that inherits from either
     * \OC\BackgroundJob\Job, \OC\BackgroundJob\QueuedJob or
     * \OC\BackgroundJob\TimedJob and register it using ->add($job, $argument),
     * $argument will be passed to the run() function of the job when the job is
     * executed.
     *
     * A regular job will be executed every time cron.php is run, a QueuedJob will
     * only run once and a TimedJob will only run at a specific interval which is to
     * be specified in the constructor of the job by calling
     * $this->setInterval($interval) with $interval in seconds.
     *
     * @package OCP\BackgroundJob
     * @since 7.0.0
     */
    public interface IJobList
    {
        /**
         * Add a job to the list
         *
         * @param \OCP\BackgroundJob\IJob|string $job
         * @param mixed $argument The argument to be passed to $job->run() when the job is exectured
         * @since 7.0.0
         */
        void add(IJob job, object argument = null);

        /**
         * Remove a job from the list
         *
         * @param \OCP\BackgroundJob\IJob|string $job
         * @param mixed $argument
         * @since 7.0.0
         */
        void remove(IJob job, object argument = null);
        void remove(string job, object argument = null);
        /**
         * check if a job is in the list
         *
         * @param \OCP\BackgroundJob\IJob|string $job
         * @param mixed $argument
         * @return bool
         * @since 7.0.0
         */
        bool has(IJob job, object argument);
        bool has(string job, object argument);

        /**
         * get all jobs in the list
         *
         * @return \OCP\BackgroundJob\IJob[]
         * @since 7.0.0
         * @deprecated 9.0.0 - This method is dangerous since it can cause load and
         * memory problems when creating too many instances.
         */
        IList<IJob> getAll();

        /**
         * get the next job in the list
         *
         * @return \OCP\BackgroundJob\IJob|null
         * @since 7.0.0
         */
        IJob? getNext();

        /**
         * @param int $id
         * @return \OCP\BackgroundJob\IJob|null
         * @since 7.0.0
         */
        IJob? getById(int id);

        /**
         * set the job that was last ran to the current time
         *
         * @param \OCP\BackgroundJob\IJob $job
         * @since 7.0.0
         */
        void setLastJob(IJob job);

        /**
         * Remove the reservation for a job
         *
         * @param IJob $job
         * @since 9.1.0
         */
        void unlockJob(IJob job);

        /**
         * get the id of the last ran job
         *
         * @return int
         * @since 7.0.0
         * @deprecated 9.1.0 - The functionality behind the value is deprecated, it
         *    only tells you which job finished last, but since we now allow multiple
         *    executors to run in parallel, it's not used to calculate the next job.
         */
        IJob getLastJob();

        /**
         * set the lastRun of $job to now
         *
         * @param IJob $job
         * @since 7.0.0
         */
        void setLastRun(IJob job);

        /**
         * set the run duration of $job
         *
         * @param IJob $job
         * @param $timeTaken
         * @since 12.0.0
         */
        void setExecutionTime(IJob job, int timeTaken);
    }

}
