namespace OCP.AppFramework.Http
{
/**
 * A renderer for JSON calls
 * @since 6.0.0
 */
    class JSONResponse extends Response {

    /**
     * response data
     * @var array|object
     */
    protected data;


    /**
     * constructor of JSONResponse
     * @param array|object data the object or array that should be transformed
     * @param int statusCode the Http status code, defaults to 200
     * @since 6.0.0
     */
    public function __construct(data=array(), statusCode=Http::STATUS_OK) {
        this->data = data;
        this->setStatus(statusCode);
        this->addHeader('Content-Type', 'application/json; charset=utf-8');
    }


    /**
     * Returns the rendered json
     * @return string the rendered json
     * @since 6.0.0
     * @throws \Exception If data could not get encoded
     */
    public function render() {
        response = json_encode(this->data, JSON_HEX_TAG);
        if(response === false) {
            throw new \Exception(sprintf('Could not json_encode due to invalid ' .
            'non UTF-8 characters in the array: %s', var_export(this->data, true)));
        }

        return response;
    }

    /**
     * Sets values in the data json array
     * @param array|object data an array or object which will be transformed
     *                             to JSON
     * @return JSONResponse Reference to this object
     * @since 6.0.0 - return value was added in 7.0.0
     */
    public function setData(data){
        this->data = data;

        return this;
    }


    /**
     * Used to get the set parameters
     * @return array the data
     * @since 6.0.0
     */
    public function getData(){
        return this->data;
    }

    }

}