using System;
using System.Linq;
using OCP.Files.Storage;
namespace OCP.WorkflowEngine
{
    /**
 * Interface IManager
 *
 * @package OCP\WorkflowEngine
 * @since 9.1
 */
public interface IManager {
	/**
	 * @param IStorage $storage
	 * @param string $path
	 * @since 9.1
	 */
	void setFileInfo(IStorage storage, string path);

	/**
	 * @param string $class
	 * @param bool $returnFirstMatchingOperationOnly
	 * @return array
	 * @since 9.1
	 */
	PhpArray getMatchingOperations(string @class, bool returnFirstMatchingOperationOnly = true);
}

}