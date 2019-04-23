using System;
namespace OCP.Migration
{
    /**
     * Interface IOutput
     *
     * @package OCP\Migration
     * @since 9.1.0
     */
    interface IOutput
    {

        /**
         * @param string message
         * @since 9.1.0
         */
        void info(string message);

        /**
         * @param string message
         * @since 9.1.0
         */
        void warning(string message);

        /**
         * @param int max
         * @since 9.1.0
         */
        void startProgress(int max = 0);

        /**
         * @param int step
         * @param string description
         * @since 9.1.0
         */
        void advance(int step = 1, string description = "");

        /**
         * @param int max
         * @since 9.1.0
         */
        void finishProgress();

    }

}
