using System;
using System.Collections;
using System.Collections.Generic;

namespace OCP.SystemTag
{
    /**
     * Public interface to access and manage system-wide tags.
     *
     * @since 9.0.0
     */
    public interface ISystemTagManager
    {

        /**
         * Returns the tag objects matching the given tag ids.
         *
         * @param array|string tagIds id or array of unique ids of the tag to retrieve
         *
         * @return ISystemTag[] array of system tags with tag id as key
         *
         * @throws \InvalidArgumentException if at least one given tag ids is invalid (string instead of integer, etc.)
         * @throws TagNotFoundException if at least one given tag ids did no exist
         *          The message contains a json_encoded array of the ids that could not be found
         *
         * @since 9.0.0
         */
        IList<ISystemTag> getTagsByIds(IList<string> tagIds);

    /**
     * Returns the tag object matching the given attributes.
     *
     * @param string tagName tag name
     * @param bool userVisible whether the tag is visible by users
     * @param bool userAssignable whether the tag is assignable by users
     *
     * @return ISystemTag system tag
     *
     * @throws TagNotFoundException if tag does not exist
     *
     * @since 9.0.0
     */
    ISystemTag getTag(string tagName, bool userVisible, bool userAssignable);

    /**
     * Creates the tag object using the given attributes.
     *
     * @param string tagName tag name
     * @param bool userVisible whether the tag is visible by users
     * @param bool userAssignable whether the tag is assignable by users
     *
     * @return ISystemTag system tag
     *
     * @throws TagAlreadyExistsException if tag already exists
     *
     * @since 9.0.0
     */
    ISystemTag createTag(string tagName, bool userVisible, bool userAssignable);

    /**
     * Returns all known tags, optionally filtered by visibility.
     *
     * @param bool|null visibilityFilter filter by visibility if non-null
     * @param string nameSearchPattern optional search pattern for the tag name
     *
     * @return ISystemTag[] array of system tags or empty array if none found
     *
     * @since 9.0.0
     */
    IList<ISystemTag> getAllTags(bool? visibilityFilter = null, string nameSearchPattern = null);

    /**
     * Updates the given tag
     *
     * @param string tagId tag id
     * @param string newName the new tag name
     * @param bool userVisible whether the tag is visible by users
     * @param bool userAssignable whether the tag is assignable by users
     *
     * @throws TagNotFoundException if tag with the given id does not exist
     * @throws TagAlreadyExistsException if there is already another tag
     * with the same attributes
     *
     * @since 9.0.0
     */
    void updateTag(string tagId, string newName, bool userVisible, bool userAssignable);

        /**
         * Delete the given tags from the database and all their relationships.
         *
         * @param string|array tagIds array of tag ids
         *
         * @throws TagNotFoundException if at least one tag did not exist
         *
         * @since 9.0.0
         */
        void deleteTags(IList<ISystemTag> tagIds);

        /**
         * Checks whether the given user is allowed to assign/unassign the tag with the
         * given id.
         *
         * @param ISystemTag tag tag to check permission for
         * @param IUser user user to check permission for
         *
         * @return true if the user is allowed to assign/unassign the tag, false otherwise
         *
         * @since 9.1.0
         */
        bool canUserAssignTag(ISystemTag tag, OCP.IUser user);

    /**
     * Checks whether the given user is allowed to see the tag with the given id.
     *
     * @param ISystemTag tag tag to check permission for
     * @param IUser user user to check permission for
     *
     * @return true if the user can see the tag, false otherwise
     *
     * @since 9.1.0
     */
    bool canUserSeeTag(ISystemTag tag, OCP.IUser user);

    /**
     * Set groups that can assign a given tag.
     *
     * @param ISystemTag tag tag for group assignment
     * @param string[] groupIds group ids of groups that can assign/unassign the tag
     *
     * @since 9.1.0
     */
    void setTagGroups(ISystemTag tag, IList<string> groupIds);

        /**
         * Get groups that can assign a given tag.
         *
         * @param ISystemTag tag tag for group assignment
         *
         * @return string[] group ids of groups that can assign/unassign the tag
         *
         * @since 9.1.0
         */
        IList<string> getTagGroups(ISystemTag tag);
}

}
