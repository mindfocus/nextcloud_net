using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Storage
{
    /**
     * Interface that adds the ability to write a stream directly to file
     *
     * @since 15.0.0
     */
    public interface IWriteStreamStorage : IStorage
    {
    /**
	 * Write the data from a stream to a file
	 *
	 * @param string path
	 * @param resource stream
	 * @param int|null size the size of the stream if known in advance
	 * @return int the number of bytes written
	 * @since 15.0.0
	 */
    int  writeStream(string path, int stream, int? size = null);
}

}
