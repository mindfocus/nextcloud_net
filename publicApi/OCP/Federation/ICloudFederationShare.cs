using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Federation
{
/**
 * Interface ICloudFederationShare
 *
 * @package OCP\Federation
 *
 * @since 14.0.0
 */
public interface ICloudFederationShare {

	/**
	 * set uid of the recipient
	 *
	 * @param string user
	 *
	 * @since 14.0.0
	 */
	void setShareWith(string user);

	/**
	 * set resource name (e.g. file, calendar, contact,...)
	 *
	 * @param string name
	 *
	 * @since 14.0.0
	 */
	void setResourceName(string name);

	/**
	 * set resource type (e.g. file, calendar, contact,...)
	 *
	 * @param string resourceType
	 *
	 * @since 14.0.0
	 */
	void setResourceType(string resourceType);

	/**
	 * set resource description (optional)
	 *
	 * @param string description
	 *
	 * @since 14.0.0
	 */
	void setDescription(string description);

	/**
	 * set provider ID (e.g. file ID)
	 *
	 * @param string providerId
	 *
	 * @since 14.0.0
	 */
	void setProviderId(string providerId);

	/**
	 * set owner UID
	 *
	 * @param string owner
	 *
	 * @since 14.0.0
	 */
	void setOwner(string owner);

	/**
	 * set owner display name
	 *
	 * @param string ownerDisplayName
	 *
	 * @since 14.0.0
	 */
	void setOwnerDisplayName(string ownerDisplayName);

	/**
	 * set UID of the user who sends the share
	 *
	 * @param string sharedBy
	 *
	 * @since 14.0.0
	 */
	void setSharedBy(string sharedBy);

	/**
	 * set display name of the user who sends the share
	 *
	 * @param sharedByDisplayName
	 *
	 * @since 14.0.0
	 */
	void setSharedByDisplayName(string sharedByDisplayName);

	/**
	 * set protocol specification
	 *
	 * @param array protocol
	 *
	 * @since 14.0.0
	 */
	void setProtocol(IList<string> protocol);

	/**
	 * share type (group or user)
	 *
	 * @param string shareType
	 *
	 * @since 14.0.0
	 */
	void setShareType(string shareType);

	/**
	 * get the whole share, ready to send out
	 *
	 * @return array
	 *
	 * @since 14.0.0
	 */
	IList<string> getShare();

	/**
	 * get uid of the recipient
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getShareWith();

	/**
	 * get resource name (e.g. file, calendar, contact,...)
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getResourceName();

	/**
	 * get resource type (e.g. file, calendar, contact,...)
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getResourceType();

	/**
	 * get resource description (optional)
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getDescription();

	/**
	 * get provider ID (e.g. file ID)
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getProviderId();

	/**
	 * get owner UID
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getOwner();

	/**
	 * get owner display name
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getOwnerDisplayName();

	/**
	 * get UID of the user who sends the share
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getSharedBy();

	/**
	 * get display name of the user who sends the share
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getSharedByDisplayName();

	/**
	 * get share type (group or user)
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getShareType();

	/**
	 * get share Secret
	 *
	 * @return string
	 *
	 * @since 14.0.0
	 */
	string getShareSecret();


	/**
	 * get protocol specification
	 *
	 * @return array
	 *
	 * @since 14.0.0
	 */
	IList<string> getProtocol();

}

}
