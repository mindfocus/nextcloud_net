namespace OCP.Comments
{
/**
 * Interface ICommentsEventHandler
 *
 * @package OCP\Comments
 * @since 11.0.0
 */
    public interface ICommentsEventHandler {

        /**
         * @param CommentsEvent event
         * @since 11.0.0
         */
        void handle(CommentsEvent @event);
    }

}