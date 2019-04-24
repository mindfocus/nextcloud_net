namespace OCP.AppFramework.Http
{
/**
 * A renderer for OCS responses
 * @since 8.1.0
 * @deprecated 9.2.0 To implement an OCS endpoint extend the OCSController
 */
    class OCSResponse extends Response {

    private data;
    private format;
    private statuscode;
    private message;
    private itemscount;
    private itemsperpage;

    /**
     * generates the xml or json response for the API call from an multidimenional data array.
     * @param string format
     * @param int statuscode
     * @param string message
     * @param array data
     * @param int|string itemscount
     * @param int|string itemsperpage
     * @since 8.1.0
     * @deprecated 9.2.0 To implement an OCS endpoint extend the OCSController
     */
    public function __construct(format, statuscode, message,
        data=[], itemscount='',
    itemsperpage='') {
        this->format = format;
        this->statuscode = statuscode;
        this->message = message;
        this->data = data;
        this->itemscount = itemscount;
        this->itemsperpage = itemsperpage;

        // set the correct header based on the format parameter
        if (format === 'json') {
            this->addHeader(
                'Content-Type', 'application/json; charset=utf-8'
            );
        } else {
            this->addHeader(
                'Content-Type', 'application/xml; charset=utf-8'
            );
        }
    }

    /**
     * @return string
     * @since 8.1.0
     * @deprecated 9.2.0 To implement an OCS endpoint extend the OCSController
     * @suppress PhanDeprecatedClass
     */
    public function render() {
        r = new \OC\OCS\Result(this->data, this->statuscode, this->message);
        r->setTotalItems(this->itemscount);
        r->setItemsPerPage(this->itemsperpage);

        return \OC_API::renderResult(this->format, r->getMeta(), r->getData());
    }


    }
}