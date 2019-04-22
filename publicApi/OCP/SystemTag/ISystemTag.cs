using System;
namespace OCP.SystemTag
{
    /**
     * Public interface for a system-wide tag.
     *
     * @since 9.0.0
     */
    public interface ISystemTag
    {

        /**
         * Returns the tag id
         *
         * @return string id
         *
         * @since 9.0.0
         */
        string getId();

        /**
         * Returns the tag display name
         *
         * @return string tag display name
         *
         * @since 9.0.0
         */
        string getName();

        /**
         * Returns whether the tag is visible for regular users
         *
         * @return bool true if visible, false otherwise
         *
         * @since 9.0.0
         */
        bool isUserVisible();

        /**
         * Returns whether the tag can be assigned to objects by regular users
         *
         * @return bool true if assignable, false otherwise
         *
         * @since 9.0.0
         */
        bool isUserAssignable();

}


}
