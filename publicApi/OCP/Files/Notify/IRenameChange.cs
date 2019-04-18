using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Notify
{
    /**
 * Represents a detected rename change
 *
 * @since 12.0.0
 */
public interface IRenameChange : IChange {
	/**
	 * Get the new path of the renamed file relative to the storage root
	 *
	 * @return string
	 *
	 * @since 12.0.0
	 */
	string getTargetPath();
}

}