namespace OCP.AppFramework.Http
{
/**
 * Class DataDisplayResponse
 *
 * @package OCP\AppFramework\Http
 * @since 8.1.0
 */
    class DataDisplayResponse : Response {

    /**
     * response data
     * @var string
     */
    protected data;


    /**
     * @param string data the data to display
     * @param int statusCode the Http status code, defaults to 200
     * @param array headers additional key value based headers
     * @since 8.1.0
     */
    public function __construct(data='', statusCode=Http::STATUS_OK,
    headers=[]) {
        this.data = data;
        this.setStatus(statusCode);
        this.setHeaders(array_merge(this.getHeaders(), headers));
        this.addHeader('Content-Disposition', 'inline; filename=""');
    }

    /**
     * Outputs data. No processing is done.
     * @return string
     * @since 8.1.0
     */
    public function render() {
        return this.data;
    }


    /**
     * Sets values in the data
     * @param string data the data to display
     * @return DataDisplayResponse Reference to this object
     * @since 8.1.0
     */
    public function setData(data){
        this.data = data;

        return this;
    }


    /**
     * Used to get the set parameters
     * @return string the data
     * @since 8.1.0
     */
    public function getData(){
        return this.data;
    }

    }

}