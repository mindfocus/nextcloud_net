namespace OCP.AppFramework.Http
{
/**
 * Class StreamResponse
 *
 * @package OCP\AppFramework\Http
 * @since 8.1.0
 */
    class StreamResponse : Response , ICallbackResponse {
    /** @var string */
    private filePath;

    /**
     * @param string|resource filePath the path to the file or a file handle which should be streamed
     * @since 8.1.0
     */
    public function __construct (filePath) {
        this->filePath = filePath;
    }


    /**
     * Streams the file using readfile
     *
     * @param IOutput output a small wrapper that handles output
     * @since 8.1.0
     */
    public function callback (IOutput output) {
        // handle caching
        if (output->getHttpResponseCode() !== Http::STATUS_NOT_MODIFIED) {
            if (!(is_resource(this->filePath) || file_exists(this->filePath))) {
                output->setHttpResponseCode(Http::STATUS_NOT_FOUND);
            } elseif (output->setReadfile(this->filePath) === false) {
                output->setHttpResponseCode(Http::STATUS_BAD_REQUEST);
            }
        }
    }

    }
}