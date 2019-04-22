using System;
using System.Linq;
using OCP.Files.Storage;
namespace OCP.WorkflowEngine
{
    /**
 * Interface ICheck
 *
 * @package OCP\WorkflowEngine
 * @since 9.1
 */
public interface ICheck {
	/**
	 * @param IStorage storage
	 * @param string path
	 * @since 9.1
	 */
	void setFileInfo(IStorage storage, string path);

	/**
	 * @param string operator
	 * @param string value
	 * @return bool
	 * @since 9.1
	 */
	bool executeCheck(string @operator, string value);

	/**
	 * @param string operator
	 * @param string value
	 * @throws \UnexpectedValueException
	 * @since 9.1
	 */
	void validateCheck(string @operator, string value);
}

}
