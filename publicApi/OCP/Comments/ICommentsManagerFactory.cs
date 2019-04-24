namespace OCP.Comments
{
/**
 * Interface ICommentsManagerFactory
 *
 * This class is responsible for instantiating and returning an ICommentsManager
 * instance.
 *
 * @package OCP\Comments
 * @since 9.0.0
 */
    public interface ICommentsManagerFactory {

        /**
         * Constructor for the comments manager factory
         *
         * @param IServerContainer serverContainer server container
         * @since 9.0.0
         */
//        public function __construct(IServerContainer serverContainer);

        /**
         * creates and returns an instance of the ICommentsManager
         *
         * @return ICommentsManager
         * @since 9.0.0
         */
        ICommentsManager getManager();
    }

}