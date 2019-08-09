using System.IO;

namespace OCP.Files.ObjectStore
{
    /**
 * Interface IObjectStore
 *
 * @package OCP\Files\ObjectStore
 * @since 7.0.0
 */
public interface IObjectStore {

	/**
	 * @return string the container or bucket name where objects are stored
	 * @since 7.0.0
	 */
	string getStorageId();

	/**
	 * @param string urn the unified resource name used to identify the object
	 * @return resource stream with the read data
	 * @throws \Exception when something goes wrong, message will be logged
	 * @throws NotFoundException if file does not exist
	 * @since 7.0.0
	 */
	Stream readObject(string urn);

	/**
	 * @param string urn the unified resource name used to identify the object
	 * @param resource stream stream with the data to write
	 * @throws \Exception when something goes wrong, message will be logged
	 * @since 7.0.0
	 */
	void writeObject(string urn, Stream stream);

	/**
	 * @param string urn the unified resource name used to identify the object
	 * @return void
	 * @throws \Exception when something goes wrong, message will be logged
	 * @since 7.0.0
	 */
	void deleteObject(string urn);

	/**
	 * Check if an object exists in the object store
	 *
	 * @param string urn
	 * @return bool
	 * @since 16.0.0
	 */
	bool objectExists(string urn);
}

}