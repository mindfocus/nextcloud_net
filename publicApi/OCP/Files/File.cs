using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{

/**
 * Interface File
 *
 * @package OCP\Files
 * @since 6.0.0
 */
public interface File : Node {
	/**
	 * Get the content of the file as string
	 *
	 * @return string
	 * @throws \OCP\Files\NotPermittedException
	 * @since 6.0.0
	 */
	string  getContent();

	/**
	 * Write to the file from string data
	 *
	 * @param string|resource $data
	 * @throws \OCP\Files\NotPermittedException
	 * @throws \OCP\Files\GenericFileException
	 * @since 6.0.0
	 */
	void putContent(string data);

	/**
	 * Get the mimetype of the file
	 *
	 * @return string
	 * @since 6.0.0
	 */
	string  getMimeType();

	/**
	 * Open the file as stream, resulting resource can be operated as stream like the result from php's own fopen
	 *
	 * @param string $mode
	 * @return resource
	 * @throws \OCP\Files\NotPermittedException
	 * @since 6.0.0
	 */
	int fopen(string mode);

	/**
	 * Compute the hash of the file
	 * Type of hash is set with $type and can be anything supported by php's hash_file
	 *
	 * @param string $type
	 * @param bool $raw
	 * @return string
	 * @since 6.0.0
	 */
	string  hash(string type, bool raw = false);

	/**
	 * Get the stored checksum for this file
	 *
	 * @return string
	 * @since 9.0.0
	 * @throws InvalidPathException
	 * @throws NotFoundException
	 */
	string  getChecksum();

	/**
	 * Get the extension of this file
	 *
	 * @return string
	 * @since 15.0.0
	 */
	sbyte  getExtension();
}

}