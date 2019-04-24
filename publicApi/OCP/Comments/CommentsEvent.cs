namespace OCP.Comments
{
/**
 * Class CommentsEvent
 *
 * @package OCP\Comments
 * @since 9.0.0
 */
    public class CommentsEvent : ext.Event {

    const string EVENT_ADD        = @"OCP\Comments\ICommentsManager::addComment";
    const string EVENT_PRE_UPDATE = @"OCP\Comments\ICommentsManager::preUpdateComment";
    const string EVENT_UPDATE     = @"OCP\Comments\ICommentsManager::updateComment";
    const string EVENT_DELETE     = @"OCP\Comments\ICommentsManager::deleteComment";

    /** @var string */
    protected string @event;
    /** @var IComment */
    protected IComment comment;

    /**
     * DispatcherEvent constructor.
     *
     * @param string event
     * @param IComment comment
     * @since 9.0.0
     */
    public CommentsEvent(string @event, IComment comment)
    {
        this.@event = @event;
        this.comment = comment;
    }

    /**
     * @return string
     * @since 9.0.0
     */
    public string getEvent()
    {
        return this.@event;
    }

    /**
     * @return IComment
     * @since 9.0.0
     */
    public IComment getComment()
    {
        return this.comment;
    }
    }

}