using System;
namespace OCP.LDAP
{
    /**
     * Interface ILDAPProviderFactory
     *
     * This class is responsible for instantiating and returning an ILDAPProvider
     * instance.
     *
     * @package OCP\LDAP
     * @since 11.0.0
     */
    public interface ILDAPProviderFactory
    {

        /**
         * Constructor for the LDAP provider factory
         *
         * @param IServerContainer serverContainer server container
         * @since 11.0.0
         */
        //public function __construct(IServerContainer serverContainer);

        /**
         * creates and returns an instance of the ILDAPProvider
         *
         * @return ILDAPProvider
         * @since 11.0.0
         */
        ILDAPProvider getLDAPProvider();
    }

}
