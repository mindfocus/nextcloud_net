using System;
using System.Collections.Generic;
using System.Linq;
using OC.Files.Cache;
using OC.Files.SimpleFS;
using OCP.Comments;
using OCP.Files;
using OCP.Files.SimpleFS;

namespace OC.Files.AppData
{
class AppData : IAppData {

	/** @var IRootFolder */
	private IRootFolder rootFolder;

	/** @var SystemConfig */
	private SystemConfig config;

	/** @var string */
	private string appId;

	/** @var Folder */
	private Folder folder;

	/** @var (ISimpleFolder|NotFoundException)[]|CappedMemoryCache */
	private CappedMemoryCache folders;

	/**
	 * AppData constructor.
	 *
	 * @param IRootFolder rootFolder
	 * @param SystemConfig systemConfig
	 * @param string appId
	 */
	public AppData(IRootFolder rootFolder,
								SystemConfig systemConfig,
								string appId) {

		this.rootFolder = rootFolder;
		this.config = systemConfig;
		this.appId = appId;
		this.folders = new CappedMemoryCache();
	}

	private string getAppDataFolderName() {
		var instanceId = this.config.getValue("instanceid", null);
		if (instanceId == null) {
			throw new Exception("no instance id!");
		}

		return "appdata_" + instanceId;
	}

	private Folder getAppDataRootFolder() {
		var name = this.getAppDataFolderName();

		try {
			/** @var Folder node */
			var node = this.rootFolder.get(name);
			return (Folder)node;
		} catch (NotFoundException e1) {
			try {
				return this.rootFolder.newFolder(name);
			} catch (NotPermittedException e2) {
				throw new Exception("Could not get appdata folder");
			}
		}
	}

	/**
	 * @return Folder
	 * @throws \RuntimeException
	 */
	private Folder getAppDataFolder() {
		if (this.folder == null) {
			var name = this.getAppDataFolderName();

			try {
				this.folder = (Folder)this.rootFolder.get(name + "/" + this.appId);
			} catch (NotFoundException e1) {
				var appDataRootFolder = this.getAppDataRootFolder();

				try {
					this.folder = (Folder)appDataRootFolder.get(this.appId);
				} catch (NotFoundException e2) {
					try {
						this.folder = appDataRootFolder.newFolder(this.appId);
					} catch (NotPermittedException e3) {
						throw new Exception($"Could not get appdata folder for {this.appId} ");
					}
				}
			}
		}

		return this.folder;
	}

	public ISimpleFolder getFolder(string name) {
		var key = this.appId + "/" + name;
		if (this.folders.get(key) != null)
		{
			object cachedFolder = this.folders.get(key);
			if (cachedFolder is Exception cachedFolderException) {
				throw cachedFolderException;
			} else {
				return cachedFolder as ISimpleFolder;
			}
		}

		Node node = null;
		try {
			// Hardening if somebody wants to retrieve '/'
			if (name == "/") {
				node = this.getAppDataFolder();
			} else {
				var path = this.getAppDataFolderName() + "/" + this.appId + "/" + name;
				node = this.rootFolder.get(path);
			}
		} catch (NotFoundException e) {
			this.folders.set(key, e);
			throw e;
		}

		/** @var Folder node */
		var folder = new SimpleFolder((Folder)node);
		this.folders.set(key, folder);
		return folder;
	}

	public ISimpleFolder newFolder(string name) {
		var key = this.appId + "/" + name;
		folder = this.getAppDataFolder().newFolder(name);

		var simpleFolder = new SimpleFolder(folder);
		this.folders.set(key, simpleFolder);
		return simpleFolder;
	}

	public IList<ISimpleFolder> getDirectoryListing() {
		var listing = this.getAppDataFolder().getDirectoryListing();


		var fileListing = listing.ToList().ConvertAll<ISimpleFolder>(o =>
		{
			if (o is Folder)
			{
				return new SimpleFolder(folder);
			}

			return null;
		});
		fileListing = fileListing.FindAll(o => o != null);

		return fileListing;
	}

	public int getId() {
		return this.getAppDataFolder().getId();
	}
}

}