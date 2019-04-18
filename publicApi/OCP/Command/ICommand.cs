using System;
namespace OCP.Command
{
    /**
     * Interface ICommand
     *
     * @package OCP\Command
     * @since 8.1.0
     */
    public interface ICommand
    {
        /**
         * Run the command
         * @since 8.1.0
         */
        public void handle();
    }

}
