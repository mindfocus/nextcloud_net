using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using publicApi.AppFramework.Utility;
using Quartz;

namespace publicApi.BackgroundJob
{
    abstract class Job : IJob
    {
        /** @var int $id */
        protected int id;

	/** @var int $lastRun */
	protected int lastRun;

	/** @var mixed $argument */
	protected object argument;

	/** @var ITimeFactory */
	    protected ITimeFactory time;
        public Job(ITimeFactory time)
        {
            this.time = time;
        }
        //public Task Execute(IJobExecutionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        public void execute(IJobList jobList, ILogger logger = null)
        {
            jobList.setLastRun(this);
            if (logger == null) {
			    //logger = \OC::$server->getLogger();
            }

            try
            {
			 var jobStartTime = this.time.getTime();
			//logger.debug('Run '.get_class($this). ' job with ID '. $this->getId(), ['app' => 'cron']);
			this.run(this.argument);
			var timeTaken = this.time.getTime() - jobStartTime;

			//$logger->debug('Finished '.get_class($this). ' job with ID '. $this->getId(). ' in '. $timeTaken. ' seconds', ['app' => 'cron']);
			jobList.setExecutionTime(this, timeTaken);
            }
            catch (Exception e) {
                if (logger != null) {
				//logger.logException(e, [
    //                'app' => 'core',
    //                'message' => 'Error while running background job (class: '.get_class($this). ', arguments: '.print_r($this->argument, true). ')'
    //            ]);
                }
            }
            }

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }

        public object getArgument()
        {
            return this.argument;
        }

        public int getId()
        {
            return this.id;
        }

        public int getLastRun()
        {
            return this.lastRun;
        }

        public void setArgument(object argument)
        {
            this.argument = argument;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public void setLastRun(int lastRun)
        {
            this.lastRun = lastRun;
        }
        /**
 * The actual function that is called to run the job
 *
 * @param $argument
 * @return mixed
 *
 * @since 15.0.0
 */
        abstract protected object run(object argument);
    }
}
