using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Notify
{
    /**
 * Provides access to detected changes in the storage by either actively listening
 * or getting the list of changes that happened in the background
 *
 * @since 12.0.0
 */
public interface INotifyHandler {
	/**
	 * Start listening for update notifications
	 *
	 * The provided callback will be called for every incoming notification with the following parameters
	 *  - IChange|IRenameChange change
	 *
	 * Note that this call is blocking and will not exit on it's own, to stop listening for notifications return `false` from the callback
	 *
	 * @param callable callback
	 *
	 * @since 12.0.0
	 */
	void listen(Action callback);

	/**
	 * Get all changes detected since the start of the notify process or the last call to getChanges
	 *
	 * @return IChange[]
	 *
	 * @since 12.0.0
	 */
	IList<IChange> getChanges();

	/**
	 * Stop listening for changes
	 *
	 * Note that any pending changes will be discarded
	 *
	 * @since 12.0.0
	 */
	void stop();
}

}