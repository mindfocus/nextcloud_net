using System;
using System.Collections.Generic;

namespace OCP.Comments
{
/**
 * Interface ICommentsManager
 *
 * This class manages the access to comments
 *
 * @package OCP\Comments
 * @since 9.0.0
 */
public interface ICommentsManager {

	/**
	 * @const DELETED_USER type and id for a user that has been deleted
	 * @see deleteReferencesOfActor
	 * @since 9.0.0
	 *
	 * To be used as replacement for user type actors in deleteReferencesOfActor().
	 *
	 * User interfaces shall show "Deleted user" as display name, if needed.
	 */
//	const string DELETED_USER = "deleted_users";

	/**
	 * returns a comment instance
	 *
	 * @param string id the ID of the comment
	 * @return IComment
	 * @throws NotFoundException
	 * @since 9.0.0
	 */
	IComment get(string id);

	/**
	 * returns the comment specified by the id and all it's child comments
	 *
	 * @param string id
	 * @param int limit max number of entries to return, 0 returns all
	 * @param int offset the start entry
	 * @return array
	 * @since 9.0.0
	 *
	 * The return array looks like this
	 * [
	 * 	 'comment' => IComment, // root comment
	 *   'replies' =>
	 *   [
	 *     0 =>
	 *     [
	 *       'comment' => IComment,
	 *       'replies' =>
	 *       [
	 *         0 =>
	 *         [
	 *           'comment' => IComment,
	 *           'replies' => [ … ]
	 *         ],
	 *         …
	 *       ]
	 *     ]
	 *     1 =>
	 *     [
	 *       'comment' => IComment,
	 *       'replies'=> [ … ]
	 *     ],
	 *     …
	 *   ]
	 * ]
	 */
	IList<string> getTree(string id, int limit = 0, int offset = 0);

	/**
	 * returns comments for a specific object (e.g. a file).
	 *
	 * The sort order is always newest to oldest.
	 *
	 * @param string objectType the object type, e.g. 'files'
	 * @param string objectId the id of the object
	 * @param int limit optional, number of maximum comments to be returned. if
	 * not specified, all comments are returned.
	 * @param int offset optional, starting point
	 * @param \DateTime|null notOlderThan optional, timestamp of the oldest comments
	 * that may be returned
	 * @return IComment[]
	 * @since 9.0.0
	 */
	IList<IComment> getForObject(
			string objectType,
			string objectId,
			int limit = 0,
			int offset = 0,
			DateTime? notOlderThan = null
	);

	/**
	 * @param string objectType the object type, e.g. 'files'
	 * @param string objectId the id of the object
	 * @param int lastKnownCommentId the last known comment (will be used as offset)
	 * @param string sortDirection direction of the comments (`asc` or `desc`)
	 * @param int limit optional, number of maximum comments to be returned. if
	 * set to 0, all comments are returned.
	 * @return IComment[]
	 * @since 14.0.0
	 */
	 IList<string> getForObjectSince(
		string objectType,
		string objectId,
		int lastKnownCommentId,
		string sortDirection = "asc",
		int limit = 30
	);

	/**
	 * Search for comments with a given content
	 *
	 * @param string search content to search for
	 * @param string objectType Limit the search by object type
	 * @param string objectId Limit the search by object id
	 * @param string verb Limit the verb of the comment
	 * @param int offset
	 * @param int limit
	 * @return IComment[]
	 * @since 14.0.0
	 */
	IList<IComment> search(string search, string objectType, string objectId, string verb, int offset, int limit = 50);

	/**
	 * @param objectType string the object type, e.g. 'files'
	 * @param objectId string the id of the object
	 * @param \DateTime|null notOlderThan optional, timestamp of the oldest comments
	 * that may be returned
	 * @param string verb Limit the verb of the comment - Added in 14.0.0
	 * @return Int
	 * @since 9.0.0
	 */
	int getNumberOfCommentsForObject(string objectType, string objectId, DateTime? notOlderThan = null,string  verb = "");

	/**
	 * Get the number of unread comments for all files in a folder
	 *
	 * @param int folderId
	 * @param IUser user
	 * @return array [fileId => unreadCount]
	 * @since 12.0.0
	 */
	IList<string> getNumberOfUnreadCommentsForFolder(int folderId, IUser user);

	/**
	 * creates a new comment and returns it. At this point of time, it is not
	 * saved in the used data storage. Use save() after setting other fields
	 * of the comment (e.g. message or verb).
	 *
	 * @param string actorType the actor type (e.g. 'users')
	 * @param string actorId a user id
	 * @param string objectType the object type the comment is attached to
	 * @param string objectId the object id the comment is attached to
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment create(string actorType,string  actorId, string objectType, string objectId);

	/**
	 * permanently deletes the comment specified by the ID
	 *
	 * When the comment has child comments, their parent ID will be changed to
	 * the parent ID of the item that is to be deleted.
	 *
	 * @param string id
	 * @return bool
	 * @since 9.0.0
	 */
	bool delete(string id);

	/**
	 * saves the comment permanently
	 *
	 * if the supplied comment has an empty ID, a new entry comment will be
	 * saved and the instance updated with the new ID.
	 *
	 * Otherwise, an existing comment will be updated.
	 *
	 * Throws NotFoundException when a comment that is to be updated does not
	 * exist anymore at this point of time.
	 *
	 * @param IComment comment
	 * @return bool
	 * @throws NotFoundException
	 * @since 9.0.0
	 */
	bool save(IComment comment);

	/**
	 * removes references to specific actor (e.g. on user delete) of a comment.
	 * The comment itself must not get lost/deleted.
	 *
	 * A 'users' type actor (type and id) should get replaced by the
	 * value of the DELETED_USER constant of this interface.
	 *
	 * @param string actorType the actor type (e.g. 'users')
	 * @param string actorId a user id
	 * @return boolean
	 * @since 9.0.0
	 */
	bool deleteReferencesOfActor(string actorType, string actorId);

	/**
	 * deletes all comments made of a specific object (e.g. on file delete)
	 *
	 * @param string objectType the object type (e.g. 'files')
	 * @param string objectId e.g. the file id
	 * @return boolean
	 * @since 9.0.0
	 */
	bool deleteCommentsAtObject(string objectType, string objectId);

	/**
	 * sets the read marker for a given file to the specified date for the
	 * provided user
	 *
	 * @param string objectType
	 * @param string objectId
	 * @param \DateTime dateTime
	 * @param \OCP\IUser user
	 * @since 9.0.0
	 */
	void setReadMark(string objectType, string objectId, DateTime dateTime, OCP.IUser user);

	/**
	 * returns the read marker for a given file to the specified date for the
	 * provided user. It returns null, when the marker is not present, i.e.
	 * no comments were marked as read.
	 *
	 * @param string objectType
	 * @param string objectId
	 * @param \OCP\IUser user
	 * @return \DateTime|null
	 * @since 9.0.0
	 */
	DateTime getReadMark(string objectType, string objectId, OCP.IUser user);

	/**
	 * deletes the read markers for the specified user
	 *
	 * @param \OCP\IUser user
	 * @return bool
	 * @since 9.0.0
	 */
	bool deleteReadMarksFromUser(OCP.IUser user);

	/**
	 * deletes the read markers on the specified object
	 *
	 * @param string objectType
	 * @param string objectId
	 * @return bool
	 * @since 9.0.0
	 */
	bool deleteReadMarksOnObject(string objectType, string objectId);

	/**
	 * registers an Entity to the manager, so event notifications can be send
	 * to consumers of the comments infrastructure
	 *
	 * @param \Closure closure
	 * @since 11.0.0
	 */
	void registerEventHandler(Action closure);

	/**
	 * registers a method that resolves an ID to a display name for a given type
	 *
	 * @param string type
	 * @param \Closure closure
	 * @throws \OutOfBoundsException
	 * @since 11.0.0
	 *
	 * Only one resolver shall be registered per type. Otherwise a
	 * \OutOfBoundsException has to thrown.
	 */
	void registerDisplayNameResolver(string type, Action closure);

	/**
	 * resolves a given ID of a given Type to a display name.
	 *
	 * @param string type
	 * @param string id
	 * @return string
	 * @throws \OutOfBoundsException
	 * @since 11.0.0
	 *
	 * If a provided type was not registered, an \OutOfBoundsException shall
	 * be thrown. It is upon the resolver discretion what to return of the
	 * provided ID is unknown. It must be ensured that a string is returned.
	 */
	string resolveDisplayName(string type, string id);

}

}