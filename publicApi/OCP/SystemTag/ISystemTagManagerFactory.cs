using System;

namespace OCP.SystemTag
{
    /**
     * Interface ISystemTagManagerFactory
     *
     * Factory interface for system tag managers
     *
     * @package OCP\SystemTag
     * @since 9.0.0
     */
    public interface ISystemTagManagerFactory
    {

        /**
         * Constructor for the system tag manager factory
         *
         * @param IServerContainer serverContainer server container
         * @since 9.0.0
         */
        //ISystemTagManagerFactory(IServerContainer serverContainer);

        /**
         * creates and returns an instance of the system tag manager
         *
         * @return ISystemTagManager
         * @since 9.0.0
         */
        ISystemTagManager getManager() ;

        /**
         * creates and returns an instance of the system tag object
         * mapper
         *
         * @return ISystemTagObjectMapper
         * @since 9.0.0
         */
        ISystemTagObjectMapper getObjectMapper() ;
}

}
