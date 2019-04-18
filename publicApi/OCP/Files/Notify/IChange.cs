using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Notify
{

/**
 * Represents a detected change in the storage
 *
 * @since 12.0.0
 */
public interface IChange {
	// const ADDED = 1;
	// const REMOVED = 2;
	// const MODIFIED = 3;
	// const RENAMED = 4;

	/**
	 * Get the type of the change
	 *
	 * @return int IChange::ADDED, IChange::REMOVED, IChange::MODIFIED or IChange::RENAMED
	 *
	 * @since 12.0.0
	 */
	int getType();

	/**
	 * Get the path of the file that was changed relative to the root of the storage
	 *
	 * Note, for rename changes this path is the old path for the file
	 *
	 * @return mixed
	 *
	 * @since 12.0.0
	 */
	object getPath();
}

}