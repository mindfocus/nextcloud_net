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
/**
 * Interface IOperation
 *
 * @package OCP\WorkflowEngine
 * @since 9.1
 */
public interface IOperation {
	/**
	 * @param string name
	 * @param array[] checks
	 * @param string operation
	 * @throws \UnexpectedValueException
	 * @since 9.1
	 */
	void validateOperation(string name, PhpArray checks, string operation);
}
}