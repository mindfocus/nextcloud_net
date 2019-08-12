namespace OCP.Group.Backend
{
    /**
     * @since 17.0.0
     */
    public interface IGetDisplayNameBackend:IBackend
    {
        /**
         * @since 17.0.0
         */
        string getDisplayName(string gid);
    }
}