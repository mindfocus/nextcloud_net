using System;
using System.Collections.Generic;

namespace OCP.Comments
{
/**
 * Interface IComment
 *
 * This class represents a comment
 *
 * @package OCP\Comments
 * @since 9.0.0
 */
public interface IComment {
//	const int MAX_MESSAGE_LENGTH = 1000;

	/**
	 * returns the ID of the comment
	 *
	 * It may return an empty string, if the comment was not stored.
	 * It is expected that the concrete Comment implementation gives an ID
	 * by itself (e.g. after saving).
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getId();

	/**
	 * sets the ID of the comment and returns itself
	 *
	 * It is only allowed to set the ID only, if the current id is an empty
	 * string (which means it is not stored in a database, storage or whatever
	 * the concrete implementation does), or vice versa. Changing a given ID is
	 * not permitted and must result in an IllegalIDChangeException.
	 *
	 * @param string id
	 * @return IComment
	 * @throws IllegalIDChangeException
	 * @since 9.0.0
	 */
	IComment setId(string id);

	/**
	 * returns the parent ID of the comment
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getParentId();

	/**
	 * sets the parent ID and returns itself
	 * @param string parentId
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setParentId(string parentId);

	/**
	 * returns the topmost parent ID of the comment
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getTopmostParentId();


	/**
	 * sets the topmost parent ID and returns itself
	 *
	 * @param string id
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setTopmostParentId(string id);

	/**
	 * returns the number of children
	 *
	 * @return int
	 * @since 9.0.0
	 */
	int getChildrenCount();

	/**
	 * sets the number of children
	 *
	 * @param int count
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setChildrenCount(int count);

	/**
	 * returns the message of the comment
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getMessage();

	/**
	 * sets the message of the comment and returns itself
	 *
	 * When the given message length exceeds MAX_MESSAGE_LENGTH an
	 * MessageTooLongException shall be thrown.
	 *
	 * @param string message
	 * @return IComment
	 * @throws MessageTooLongException
	 * @since 9.0.0
	 */
	IComment setMessage(string message);

	/**
	 * returns an array containing mentions that are included in the comment
	 *
	 * @return array each mention provides a 'type' and an 'id', see example below
	 * @since 11.0.0
	 *
	 * The return array looks like:
	 * [
	 *   [
	 *     'type' => 'user',
	 *     'id' => 'citizen4'
	 *   ],
	 *   [
	 *     'type' => 'group',
	 *     'id' => 'media'
	 *   ],
	 *   …
	 * ]
	 *
	 */
	IList<string> getMentions();

	/**
	 * returns the verb of the comment
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getVerb();

	/**
	 * sets the verb of the comment, e.g. 'comment' or 'like'
	 *
	 * @param string verb
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setVerb(string verb);

	/**
	 * returns the actor type
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getActorType();

	/**
	 * returns the actor ID
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getActorId();

	/**
	 * sets (overwrites) the actor type and id
	 *
	 * @param string actorType e.g. 'users'
	 * @param string actorId e.g. 'zombie234'
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setActor(string actorType, string actorId);

	/**
	 * returns the creation date of the comment.
	 *
	 * If not explicitly set, it shall default to the time of initialization.
	 *
	 * @return \DateTime
	 * @since 9.0.0
	 */
	DateTime getCreationDateTime();

	/**
	 * sets the creation date of the comment and returns itself
	 *
	 * @param \DateTime dateTime
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setCreationDateTime(DateTime dateTime);

	/**
	 * returns the date of the most recent child
	 *
	 * @return \DateTime
	 * @since 9.0.0
	 */
	DateTime getLatestChildDateTime();

	/**
	 * sets the date of the most recent child
	 *
	 * @param \DateTime dateTime
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setLatestChildDateTime(DateTime dateTime);

	/**
	 * returns the object type the comment is attached to
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getObjectType();

	/**
	 * returns the object id the comment is attached to
	 *
	 * @return string
	 * @since 9.0.0
	 */
	string getObjectId();

	/**
	 * sets (overwrites) the object of the comment
	 *
	 * @param string objectType e.g. 'files'
	 * @param string objectId e.g. '16435'
	 * @return IComment
	 * @since 9.0.0
	 */
	IComment setObject(string objectType, string objectId);

}


}