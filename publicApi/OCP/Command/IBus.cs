using System;
namespace publicApi.Command
{
    /**
     * Interface IBus
     *
     * @package OCP\Command
     * @since 8.1.0
     */
    public interface IBus
    {
        /*
         * Schedule a command to be fired
         *
         * @param \OCP\Command\ICommand | callable $command
         * @since 8.1.0
         */
        void push(ICommand command);

        /*
         * Require all commands using a trait to be run synchronous
         *
         * @param string $trait
         * @since 8.1.0
         */
        //void requireSync($trait);
    }

}
