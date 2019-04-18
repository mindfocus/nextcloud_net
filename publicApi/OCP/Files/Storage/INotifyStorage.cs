using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Storage
{
    /**
     * Storage backend that support active notifications
     *
     * @since 9.1.0
     */
    public interface INotifyStorage
    {
        //const NOTIFY_ADDED = 1;
        //const NOTIFY_REMOVED = 2;
        //const NOTIFY_MODIFIED = 3;
        //const NOTIFY_RENAMED = 4;

        /**
         * Start listening for update notifications
         *
         * The provided callback will be called for every incoming notification with the following parameters
         *  - int $type the type of update, one of the INotifyStorage::NOTIFY_* constants
         *  - string $path the path of the update
         *  - string $renameTarget the target of the rename operation, only provided for rename updates
         *
         * Note that this call is blocking and will not exit on it's own, to stop listening for notifications return `false` from the callback
         *
         * @param string $path
         * @param callable $callback
         *
         * @since 9.1.0
         * @deprecated 12.0.0 use INotifyStorage::notify()->listen() instead
         */
        void listen(string path, Action callback);

        /**
         * Start the notification handler for this storage
         *
         * @param $path
         * @return INotifyHandler
         *
         * @since 12.0.0
         */
        INotifyHandler notify(string path);
    }

}
