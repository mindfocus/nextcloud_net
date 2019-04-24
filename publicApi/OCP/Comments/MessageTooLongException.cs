namespace OCP.Comments
{
/**
 * Exception thrown when a comment message exceeds the allowed character limit
 * @since 9.0.0
 */
    public class MessageTooLongException : System.OverflowException {}
}