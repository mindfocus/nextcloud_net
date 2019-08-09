using OCP.Files.Mount;

namespace OC.Files.Mount
{

class MountPoint : IMountPoint {
	/**
	 * @var OC.Files.Storage.Storage storage
	 */
	protected storage = null;
	protected class;
	protected storageId;
	protected rootId = null;

	/**
	 * Configuration options for the storage backend
	 *
	 * @var array
	 */
	protected arguments = array();
	protected mountPoint;

	/**
	 * Mount specific options
	 *
	 * @var array
	 */
	protected mountOptions = array();

	/**
	 * @var .OC.Files.Storage.StorageFactory loader
	 */
	private loader;

	/**
	 * Specified whether the storage is invalid after failing to
	 * instantiate it.
	 *
	 * @var bool
	 */
	private invalidStorage = false;

	/** @var int|null */
	protected mountId;

	/**
	 * @param string|.OC.Files.Storage.Storage storage
	 * @param string mountpoint
	 * @param array arguments (optional) configuration for the storage backend
	 * @param .OCP.Files.Storage.IStorageFactory loader
	 * @param array mountOptions mount specific options
	 * @param int|null mountId
	 * @throws .Exception
	 */
	public function __construct(storage, mountpoint, arguments = null, loader = null, mountOptions = null, mountId = null) {
		if (is_null(arguments)) {
			arguments = array();
		}
		if (is_null(loader)) {
			this.loader = new StorageFactory();
		} else {
			this.loader = loader;
		}

		if (!is_null(mountOptions)) {
			this.mountOptions = mountOptions;
		}

		mountpoint = this.formatPath(mountpoint);
		this.mountPoint = mountpoint;
		if (storage instanceof Storage) {
			this.class = get_class(storage);
			this.storage = this.loader.wrap(this, storage);
		} else {
			// Update old classes to new namespace
			if (strpos(storage, 'OC_Filestorage_') !== false) {
				storage = '.OC.Files.Storage..' . substr(storage, 15);
			}
			this.class = storage;
			this.arguments = arguments;
		}
		this.mountId = mountId;
	}

	/**
	 * get complete path to the mount point, relative to data/
	 *
	 * @return string
	 */
	public string getMountPoint() {
		return this.mountPoint;
	}

	/**
	 * Sets the mount point path, relative to data/
	 *
	 * @param string mountPoint new mount point
	 */
	public function setMountPoint(mountPoint) {
		this.mountPoint = this.formatPath(mountPoint);
	}

	/**
	 * create the storage that is mounted
	 */
	private function createStorage() {
		if (this.invalidStorage) {
			return;
		}

		if (class_exists(this.class)) {
			try {
				class = this.class;
				// prevent recursion by setting the storage before applying wrappers
				this.storage = new class(this.arguments);
				this.storage = this.loader.wrap(this, this.storage);
			} catch (.Exception exception) {
				this.storage = null;
				this.invalidStorage = true;
				if (this.mountPoint === '/') {
					// the root storage could not be initialized, show the user!
					throw new .Exception('The root storage could not be initialized. Please contact your local administrator.', exception.getCode(), exception);
				} else {
					.OC::server.getLogger().logException(exception, ['level' => ILogger::ERROR]);
				}
				return;
			}
		} else {
			.OCP.Util::writeLog('core', 'storage backend ' . this.class . ' not found', ILogger::ERROR);
			this.invalidStorage = true;
			return;
		}
	}

	/**
	 * @return .OC.Files.Storage.Storage
	 */
	public OC.Files.Storage.Storage getStorage() {
		if (is_null(this.storage)) {
			this.createStorage();
		}
		return this.storage;
	}

	/**
	 * @return string
	 */
	public function getStorageId() {
		if (!this.storageId) {
			if (is_null(this.storage)) {
				storage = this.createStorage(); //FIXME: start using exceptions
				if (is_null(storage)) {
					return null;
				}

				this.storage = storage;
			}
			this.storageId = this.storage.getId();
			if (strlen(this.storageId) > 64) {
				this.storageId = md5(this.storageId);
			}
		}
		return this.storageId;
	}

	/**
	 * @return int
	 */
	public function getNumericStorageId() {
		return this.getStorage().getStorageCache().getNumericId();
	}

	/**
	 * @param string path
	 * @return string
	 */
	public function getInternalPath(path) {
		path = Filesystem::normalizePath(path, true, false, true);
		if (this.mountPoint === path or this.mountPoint . '/' === path) {
			internalPath = '';
		} else {
			internalPath = substr(path, strlen(this.mountPoint));
		}
		// substr returns false instead of an empty string, we always want a string
		return (string)internalPath;
	}

	/**
	 * @param string path
	 * @return string
	 */
	private function formatPath(path) {
		path = Filesystem::normalizePath(path);
		if (strlen(path) > 1) {
			path .= '/';
		}
		return path;
	}

	/**
	 * @param callable wrapper
	 */
	public function wrapStorage(wrapper) {
		storage = this.getStorage();
		// storage can be null if it couldn't be initialized
		if (storage != null) {
			this.storage = wrapper(this.mountPoint, storage, this);
		}
	}

	/**
	 * Get a mount option
	 *
	 * @param string name Name of the mount option to get
	 * @param mixed default Default value for the mount option
	 * @return mixed
	 */
	public function getOption(name, default) {
		return isset(this.mountOptions[name]) ? this.mountOptions[name] : default;
	}

	/**
	 * Get all options for the mount
	 *
	 * @return array
	 */
	public function getOptions() {
		return this.mountOptions;
	}

	/**
	 * Get the file id of the root of the storage
	 *
	 * @return int
	 */
	public function getStorageRootId() {
		if (is_null(this.rootId)) {
			this.rootId = (int)this.getStorage().getCache().getId('');
		}
		return this.rootId;
	}

	public function getMountId() {
		return this.mountId;
	}

	public function getMountType() {
		return '';
	}
}

}