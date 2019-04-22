using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{
    /**
 * @since 6.0.0
 */
public interface Folder : Node {
	/**
	 * Get the full path of an item in the folder within owncloud's filesystem
	 *
	 * @param string path relative path of an item in the folder
	 * @return string
	 * @throws \OCP\Files\NotPermittedException
	 * @since 6.0.0
	 */
	string  getFullPath(string path);

	/**
	 * Get the path of an item in the folder relative to the folder
	 *
	 * @param string path absolute path of an item in the folder
	 * @throws \OCP\Files\NotFoundException
	 * @return string
	 * @since 6.0.0
	 */
	string  getRelativePath(string path);

	/**
	 * check if a node is a (grand-)child of the folder
	 *
	 * @param \OCP\Files\Node node
	 * @return bool
	 * @since 6.0.0
	 */
	bool isSubNode(Node node);

	/**
	 * get the content of this directory
	 *
	 * @throws \OCP\Files\NotFoundException
	 * @return \OCP\Files\Node[]
	 * @since 6.0.0
	 */
	IList<Node> getDirectoryListing();

	/**
	 * Get the node at path
	 *
	 * @param string path relative path of the file or folder
	 * @return \OCP\Files\Node
	 * @throws \OCP\Files\NotFoundException
	 * @since 6.0.0
	 */
	Node get(string path);

	/**
	 * Check if a file or folder exists in the folder
	 *
	 * @param string path relative path of the file or folder
	 * @return bool
	 * @since 6.0.0
	 */
	bool nodeExists(string path);

	/**
	 * Create a new folder
	 *
	 * @param string path relative path of the new folder
	 * @return \OCP\Files\Folder
	 * @throws \OCP\Files\NotPermittedException
	 * @since 6.0.0
	 */
	Folder newFolder(string path);

	/**
	 * Create a new file
	 *
	 * @param string path relative path of the new file
	 * @return \OCP\Files\File
	 * @throws \OCP\Files\NotPermittedException
	 * @since 6.0.0
	 */
	File newFile(string path);

	/**
	 * search for files with the name matching query
	 *
	 * @param string|ISearchQuery query
	 * @return \OCP\Files\Node[]
	 * @since 6.0.0
	 */
	IList<Node> search(string query);
	IList<Node> search(Search.ISearchQuery query);
	/**
	 * search for files by mimetype
	 * mimetype can either be a full mimetype (image/png) or a wildcard mimetype (image)
	 *
	 * @param string mimetype
	 * @return \OCP\Files\Node[]
	 * @since 6.0.0
	 */
	IList<Node> searchByMime(string mimetype);

	/**
	 * search for files by tag
	 *
	 * @param string|int tag tag name or tag id
	 * @param string userId owner of the tags
	 * @return \OCP\Files\Node[]
	 * @since 8.0.0
	 */
	IList<Node> searchByTag(string tag, string userId);

	/**
	 * get a file or folder inside the folder by it's internal id
	 *
	 * This method could return multiple entries. For example once the file/folder
	 * is shared or mounted (files_external) to the user multiple times.
	 *
	 * @param int id
	 * @return \OCP\Files\Node[]
	 * @since 6.0.0
	 */
	IList<Node> getById(int id);

	/**
	 * Get the amount of free space inside the folder
	 *
	 * @return int
	 * @since 6.0.0
	 */
	int getFreeSpace();

	/**
	 * Check if new files or folders can be created within the folder
	 *
	 * @return bool
	 * @since 6.0.0
	 */
	bool isCreatable();

	/**
	 * Add a suffix to the name in case the file exists
	 *
	 * @param string name
	 * @return string
	 * @throws NotPermittedException
	 * @since 8.1.0
	 */
	string getNonExistingName(string name);

	/**
	 * @param int limit
	 * @param int offset
	 * @return \OCP\Files\Node[]
	 * @since 9.1.0
	 */
	IList<Node> getRecent(int limit, int offset = 0);
}

}