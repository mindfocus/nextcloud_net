using System;
namespace OCP.Files
{
    /**
     * Class ForbiddenException
     *
     * @package OCP\Files
     * @since 9.0.0
     */
    class ForbiddenException : Exception {

    /** @var bool */
    private bool retry;

    /**
     * @param string message
     * @param bool retry
     * @param \Exception|null previous previous exception for cascading
     * @since 9.0.0
     */
    public ForbiddenException(string message, bool retry, Exception previous = null): base(message,0,previous)
    {
            this.retry = retry;
    }

    /**
     * @return bool
     * @since 9.0.0
     */
    public bool getRetry()
    {
        return this.retry;
    }
}

}
