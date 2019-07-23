using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.SimpleFS
{
    /**
     * This class represents a file that is only hold in memory.
     *
     * @package OC\Files\SimpleFS
     * @since 16.0.0
     */
    public class InMemoryFile : ISimpleFile
    {
    /**
	 * Holds the file name.
	 *
	 * @var string
	 */
    private string name;

	/**
	 * Holds the file contents.
	 *
	 * @var string
	 */
	private string contents;

	/**
	 * InMemoryFile constructor.
	 *
	 * @param string name The file name
	 * @param string contents The file contents
	 * @since 16.0.0
	 */
	public InMemoryFile(string name, string contents)
    {
            this.name = name;
            this.contents = contents;
    }

    /**
	 * @inheritdoc
	 * @since 16.0.0
	 */
    public string getName()
    {
            return this.name;
    }

    /**
	 * @inheritdoc
	 * @since 16.0.0
	 */
    public int getSize()
    {
            return this.contents.Length;
    }

    /**
	 * @inheritdoc
	 * @since 16.0.0
	 */
    public string getETag()
    {
        return "";
    }

    /**
	 * @inheritdoc
	 * @since 16.0.0
	 */
    public long getMTime()
    {
            return DateTime.Now.Ticks;
    }

    /**
	 * @inheritdoc
	 * @since 16.0.0
	 */
    public string getContent()
    {
            return this.contents;
    }

    /**
	 * @inheritdoc
	 * @since 16.0.0
	 */
    public void putContent(string data)
    {
            this.contents = data;
    }

    /**
	 * In memory files can't be deleted.
	 *
	 * @since 16.0.0
	 */
    public void delete()
    {
        // unimplemented for in memory files
    }

    /**
	 * @inheritdoc
	 * @since 16.0.0
	 */
    public string getMimeType()
    {
            //fileInfo = new \finfo(FILEINFO_MIME_TYPE);
            //      return fileInfo.buffer(this.contents);
            return "";
    }

    /**
	 * Stream reading is unsupported for in memory files.
	 *
	 * @throws NotPermittedException
	 * @since 16.0.0
	 */
    public int read()
    {
            //throw new NotPermittedException(
            //    'Stream reading is unsupported for in memory files'
            //);
            return -1;
    }

    /**
	 * Stream writing isn't available for in memory files.
	 *
	 * @throws NotPermittedException
	 * @since 16.0.0
	 */
    public int write()
    {
            return -1;
        //throw new NotPermittedException(
        //    'Stream writing is unsupported for in memory files'
        //);
    }
}

}
