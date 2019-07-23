using System.Collections;
using System.Collections.Generic;
using OC.Files;
using OCP;
using OCP.Encryption;

namespace OC.Encryption
{

public class Util {

	const string HEADER_START = "HBEGIN";
	const string HEADER_END = "HEND";
	const char HEADER_PADDING_CHAR = '-';

	const string HEADER_ENCRYPTION_MODULE_KEY = "oc_encryption_module";

	/**
	 * block size will always be 8192 for a PHP stream
	 * @see https://bugs.php.net/bug.php?id=21641
	 * @var integer
	 */
	protected int headerSize = 8192;

	/**
	 * block size will always be 8192 for a PHP stream
	 * @see https://bugs.php.net/bug.php?id=21641
	 * @var integer
	 */
	protected int blockSize = 8192;

	/** @var View */
	protected View rootView;

	/** @var array */
	protected IList<string> ocHeaderKeys;

	/** @var .OC.User.Manager */
	protected OC.User.Manager userManager;

	/** @var IConfig */
	protected IConfig config;

	/** @var array paths excluded from encryption */
	protected IList<string> excludedPaths;

	/** @var OC.Group.Manager manager */
	protected OC.Group.Manager groupManager;

	/**
	 *
	 * @param View rootView
	 * @param .OC.User.Manager userManager
	 * @param .OC.Group.Manager groupManager
	 * @param IConfig config
	 */
	public Util(
		View rootView,
		OC.User.Manager userManager,
		OC.Group.Manager groupManager,
		IConfig config) {

		this.ocHeaderKeys = new List<string>(){
			HEADER_ENCRYPTION_MODULE_KEY
		};

		this.rootView = rootView;
		this.userManager = userManager;
		this.groupManager = groupManager;
		this.config = config;

		this.excludedPaths = new List<string>
		{
			"files_encryption",
			"appdata_" + config.getSystemValue("instanceid", null),
			"files_external"
		};
	}

	/**
	 * read encryption module ID from header
	 *
	 * @param array header
	 * @return string
	 * @throws ModuleDoesNotExistsException
	 */
	public string getEncryptionModuleId(IDictionary<string,string> header = null) {
		var id = "";
		var encryptionModuleKey = HEADER_ENCRYPTION_MODULE_KEY;
		if (header.ContainsKey(encryptionModuleKey))
		{
			id = header[encryptionModuleKey];
		}
		else if (header.ContainsKey("cipher"))
		{
			// TODO class exists
//			if (class_exists(".OCA.Encryption.Crypto.Encryption")) {
//				// fall back to default encryption if the user migrated from
//				// ownCloud <= 8.0 with the old encryption
//				id = .OCA.Encryption.Crypto.Encryption::ID;
//			} else {
//				throw new ModuleDoesNotExistsException("Default encryption module missing");
//			}
		}

		return id;

	}

	/**
	 * create header for encrypted file
	 *
	 * @param array headerData
	 * @param IEncryptionModule encryptionModule
	 * @return string
	 * @throws EncryptionHeaderToLargeException if header has to many arguments
	 * @throws EncryptionHeaderKeyExistsException if header key is already in use
	 */
	public string createHeader(IDictionary<string,string> headerData, IEncryptionModule encryptionModule) {
		var header = HEADER_START + ":" + HEADER_ENCRYPTION_MODULE_KEY + ":" + encryptionModule.getId() + ":";
		foreach (var headerKeyPair in headerData)
		{
			if (this.ocHeaderKeys.Contains(headerKeyPair.Key))
			{
				throw new EncryptionHeaderKeyExistsException();
			}

			header += headerKeyPair.Key + ":" + headerKeyPair.Value + ":";
		}

		header += HEADER_END;

		if (header.Length > this.getHeaderSize()) {
			throw new EncryptionHeaderToLargeException();
		}
		var paddedHeader = header.PadRight(this.headerSize,HEADER_PADDING_CHAR)

		return paddedHeader;
	}

	/**
	 * go recursively through a dir and collect all files and sub files.
	 *
	 * @param string dir relative to the users files folder
	 * @return array with list of files relative to the users files folder
	 */
	public IList<string> getAllFiles(string dir) {
		var result = new List<string>();
		var dirList = new Stack<string>();
		dirList.Push(dir);

		while (dirList.Count != 0)
		{
			var dirc = dirList.Pop();
			var content = this.rootView.getDirectoryContent(dirc);
			foreach (var fileInfo in content)
			{
				if (fileInfo.getType() == "dir")
				{
					dirList.Push(fileInfo.getPath());
				}
				else
				{
					result.Add(fileInfo.getPath());
				}
			}

		}

		return result;
	}

	/**
	 * check if it is a file uploaded by the user stored in data/user/files
	 * or a metadata file
	 *
	 * @param string path relative to the data/ folder
	 * @return boolean
	 */
	public bool isFile(string path) {
		parts = explode("/", Filesystem.normalizePath(path), 4);
		if (isset(parts[2]) && parts[2] === "files") {
			return true;
		}
		return false;
	}

	/**
	 * return size of encryption header
	 *
	 * @return integer
	 */
	public function getHeaderSize() {
		return this.headerSize;
	}

	/**
	 * return size of block read by a PHP stream
	 *
	 * @return integer
	 */
	public function getBlockSize() {
		return this.blockSize;
	}

	/**
	 * get the owner and the path for the file relative to the owners files folder
	 *
	 * @param string path
	 * @return array
	 * @throws .BadMethodCallException
	 */
	public function getUidAndFilename(path) {

		parts = explode("/", path);
		uid = "";
		if (count(parts) > 2) {
			uid = parts[1];
		}
		if (!this.userManager.userExists(uid)) {
			throw new .BadMethodCallException(
				"path needs to be relative to the system wide data folder and point to a user specific file"
			);
		}

		ownerPath = implode("/", array_slice(parts, 2));

		return array(uid, Filesystem::normalizePath(ownerPath));

	}

	/**
	 * Remove .path extension from a file path
	 * @param string path Path that may identify a .part file
	 * @return string File path without .part extension
	 * @note this is needed for reusing keys
	 */
	public function stripPartialFileExtension(path) {
		extension = pathinfo(path, PATHINFO_EXTENSION);

		if ( extension === "part") {

			newLength = strlen(path) - 5; // 5 = strlen(".part")
			fPath = substr(path, 0, newLength);

			// if path also contains a transaction id, we remove it too
			extension = pathinfo(fPath, PATHINFO_EXTENSION);
			if(substr(extension, 0, 12) === "ocTransferId") { // 12 = strlen("ocTransferId")
				newLength = strlen(fPath) - strlen(extension) -1;
				fPath = substr(fPath, 0, newLength);
			}
			return fPath;

		} else {
			return path;
		}
	}

	public function getUserWithAccessToMountPoint(users, groups) {
		result = [];
		if (in_array("all", users)) {
			users = this.userManager.search("", null, null);
			result = array_map(function(IUser user) {
				return user.getUID();
			}, users);
		} else {
			result = array_merge(result, users);

			groupManager = .OC::server.getGroupManager();
			foreach (groups as group) {
				groupObject = groupManager.get(group);
				if (groupObject) {
					foundUsers = groupObject.searchUsers("", -1, 0);
					userIds = [];
					foreach (foundUsers as user) {
						userIds[] = user.getUID();
					}
					result = array_merge(result, userIds);
				}
			}
		}

		return result;
	}

	/**
	 * check if the file is stored on a system wide mount point
	 * @param string path relative to /data/user with leading "/"
	 * @param string uid
	 * @return boolean
	 */
	public function isSystemWideMountPoint(path, uid) {
		if (.OCP.App::isEnabled("files_external")) {
			mounts = .OC_Mount_Config::getSystemMountPoints();
			foreach (mounts as mount) {
				if (strpos(path, "/files/" . mount["mountpoint"]) === 0) {
					if (this.isMountPointApplicableToUser(mount, uid)) {
						return true;
					}
				}
			}
		}
		return false;
	}

	/**
	 * check if mount point is applicable to user
	 *
	 * @param array mount contains mount["applicable"]["users"], mount["applicable"]["groups"]
	 * @param string uid
	 * @return boolean
	 */
	private function isMountPointApplicableToUser(mount, uid) {
		acceptedUids = array("all", uid);
		// check if mount point is applicable for the user
		intersection = array_intersect(acceptedUids, mount["applicable"]["users"]);
		if (!empty(intersection)) {
			return true;
		}
		// check if mount point is applicable for group where the user is a member
		foreach (mount["applicable"]["groups"] as gid) {
			if (this.groupManager.isInGroup(uid, gid)) {
				return true;
			}
		}
		return false;
	}

	/**
	 * check if it is a path which is excluded by ownCloud from encryption
	 *
	 * @param string path
	 * @return boolean
	 */
	public function isExcluded(path) {
		normalizedPath = Filesystem::normalizePath(path);
		root = explode("/", normalizedPath, 4);
		if (count(root) > 1) {

			// detect alternative key storage root
			rootDir = this.getKeyStorageRoot();
			if (rootDir !== "" &&
				0 === strpos(
					Filesystem::normalizePath(path),
					Filesystem::normalizePath(rootDir)
				)
			) {
				return true;
			}


			//detect system wide folders
			if (in_array(root[1], this.excludedPaths)) {
				return true;
			}

			// detect user specific folders
			if (this.userManager.userExists(root[1])
				&& in_array(root[2], this.excludedPaths)) {

				return true;
			}
		}
		return false;
	}

	/**
	 * check if recovery key is enabled for user
	 *
	 * @param string uid
	 * @return boolean
	 */
	public function recoveryEnabled(uid) {
		enabled = this.config.getUserValue(uid, "encryption", "recovery_enabled", "0");

		return enabled === "1";
	}

	/**
	 * set new key storage root
	 *
	 * @param string root new key store root relative to the data folder
	 */
	public function setKeyStorageRoot(root) {
		this.config.setAppValue("core", "encryption_key_storage_root", root);
	}

	/**
	 * get key storage root
	 *
	 * @return string key storage root
	 */
	public function getKeyStorageRoot() {
		return this.config.getAppValue("core", "encryption_key_storage_root", "");
	}

}

}