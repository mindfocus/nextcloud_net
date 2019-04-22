using System;
namespace OCP.LDAP
{
    /**
     * Interface IDeletionFlagSupport
     *
     * @package OCP\LDAP
     * @since 11.0.0
     */
    public interface IDeletionFlagSupport
    {
        /**
         * Flag record for deletion.
         * @param string uid user id
         * @since 11.0.0
         */
        void flagRecord(string uid);

        /**
         * Unflag record for deletion.
         * @param string uid user id
         * @since 11.0.0
         */
        void unflagRecord(string uid);
    }

}
