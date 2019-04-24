namespace OCP.Collaboration.Resources
{
/**
 * @since 16.0.0
 */
    public interface IProvider {

        /**
         * Get the resource type of the provider
         *
         * @return string
         * @since 16.0.0
         */
        string getType();

        /**
         * Get the display name of a resource
         *
         * @param IResource resource
         * @return string
         * @since 16.0.0
         */
        string getName(IResource resource);

        /**
         * Get the icon class of a resource
         *
         * @param IResource resource
         * @return string
         * @since 16.0.0
         */
        string getIconClass(IResource resource);

        /**
         * Get the link to a resource
         *
         * @param IResource resource
         * @return string
         * @since 16.0.0
         */
        string getLink(IResource resource);

        /**
         * Can a user/guest access the collection
         *
         * @param IResource resource
         * @param IUser|null user
         * @return bool
         * @since 16.0.0
         */
        bool canAccessResource(IResource resource, IUser? user);

    }

}