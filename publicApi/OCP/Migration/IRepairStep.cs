using System;
namespace OCP.Migration
{
    /**
     * Repair step
     * @since 9.1.0
     */
    public interface IRepairStep
    {

        /**
         * Returns the step's name
         *
         * @return string
         * @since 9.1.0
         */
        string getName();

        /**
         * Run repair step.
         * Must throw exception on error.
         *
         * @param IOutput output
         * @throws \Exception in case of failure
         * @since 9.1.0
         */
        void run(IOutput output);

    }

}
