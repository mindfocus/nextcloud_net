using System.IO;
using System.Net.Mail;
using System.Text;
using OCP.Mail;

namespace OC.Mail
{
/**
 * Class Attachment
 *
 * @package OC\Mail
 * @since 13.0.0
 */
    class Attachment : IAttachment {

    /** @var \Swift_Mime_Attachment */
    protected FluentEmail.Core.Models.Attachment swiftAttachment;

    public Attachment(FluentEmail.Core.Models.Attachment attachment) {
        this.swiftAttachment = attachment;
    }

    /**
     * @param string filename
     * @return this
     * @since 13.0.0
     */
    public IAttachment setFilename(string filename)
    {
        this.swiftAttachment.Filename = filename;
        return this;
    }

    /**
     * @param string contentType
     * @return this
     * @since 13.0.0
     */
    public IAttachment setContentType(string contentType) {
        this.swiftAttachment.ContentType = contentType;
        return this;
    }

    /**
     * @param string body
     * @return this
     * @since 13.0.0
     */
    public IAttachment setBody(string body)
    {
        byte[] byteArray = Encoding.ASCII.GetBytes( body );
        MemoryStream stream = new MemoryStream( byteArray );
        this.swiftAttachment.Data = stream;
        return this;
    }

    /**
     * @return \Swift_Mime_Attachment
     */
    public FluentEmail.Core.Models.Attachment getSwiftAttachment() {
        return this.swiftAttachment;
    }

    }

}