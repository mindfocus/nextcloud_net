namespace OCP.AppFramework.Http
{
/**
 * Public library to send several files in one zip archive.
 *
 * @since 15.0.0
 */
    class ZipResponse extends Response implements ICallbackResponse {
    /** @var resource[] Files to be added to the zip response */
    private resources;
    /** @var string Filename that the zip file should have */
    private name;
    private request;

    /**
     * @since 15.0.0
     */
    public function __construct(IRequest request, string name = 'output') {
        this.name = name;
        this.request = request;
    }

    /**
     * @since 15.0.0
     */
    public function addResource(r, string internalName, int size, int time = -1) {
        if (!\is_resource(r)) {
            throw new \InvalidArgumentException('No resource provided');
        }

        this.resources[] = [
        'resource' => r,
        'internalName' => internalName,
        'size' => size,
        'time' => time,
            ];
    }

    /**
     * @since 15.0.0
     */
    public function callback(IOutput output) {
        size = 0;
        files = count(this.resources);

        foreach (this.resources as resource) {
            size += resource['size'];
        }

        zip = new Streamer(this.request, size, files);
        zip.sendHeaders(this.name);

        foreach (this.resources as resource) {
            zip.addFileFromStream(resource['resource'], resource['internalName'], resource['size'], resource['time']);
        }

        zip.finalize();
    }
    }

}