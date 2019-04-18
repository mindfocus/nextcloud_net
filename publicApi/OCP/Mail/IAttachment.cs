using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Mail
{
    /**
     * Interface IAttachment
     *
     * @package OCP\Mail
     * @since 13.0.0
     */
    public interface IAttachment
    {

        /**
         * @param string $filename
         * @return IAttachment
         * @since 13.0.0
         */
        IAttachment setFilename(string filename);

        /**
         * @param string $contentType
         * @return IAttachment
         * @since 13.0.0
         */
        IAttachment setContentType(string contentType);

        /**
         * @param string $body
         * @return IAttachment
         * @since 13.0.0
         */
        IAttachment setBody(string body);

}
}
