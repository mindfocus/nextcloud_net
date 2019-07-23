namespace OCP.AppFramework.Http
{
/**
 * Class FileDisplayResponse
 *
 * @package OCP\AppFramework\Http
 * @since 11.0.0
 */
    class FileDisplayResponse : Response , ICallbackResponse {

    /** @var \OCP\Files\File|\OCP\Files\SimpleFS\ISimpleFile */
    private file;

    /**
     * FileDisplayResponse constructor.
     *
     * @param \OCP\Files\File|\OCP\Files\SimpleFS\ISimpleFile file
     * @param int statusCode
     * @param array headers
     * @since 11.0.0
     */
    public function __construct(file, statusCode=Http::STATUS_OK,
    headers=[]) {
        this.file = file;
        this.setStatus(statusCode);
        this.setHeaders(array_merge(this.getHeaders(), headers));
        this.addHeader('Content-Disposition', 'inline; filename="' . rawurldecode(file.getName()) . '"');

        this.setETag(file.getEtag());
        lastModified = new \DateTime();
        lastModified.setTimestamp(file.getMTime());
        this.setLastModified(lastModified);
    }

    /**
     * @param IOutput output
     * @since 11.0.0
     */
    public function callback(IOutput output) {
        if (output.getHttpResponseCode() !== Http::STATUS_NOT_MODIFIED) {
            output.setHeader('Content-Length: ' . this.file.getSize());
            output.setOutput(this.file.getContent());
        }
    }
    }
}