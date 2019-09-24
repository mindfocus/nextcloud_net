using System.Collections;
using System.Collections.Generic;

namespace OCP.Collaboration.Resources
{
/**
 * @since 16.0.0
 */
    public interface ICollection {

        /**
         * @return int
         * @since 16.0.0
         */
        int getId();

        /**
         * @return string
         * @since 16.0.0
         */
        string getName();

        /**
         * @param string name
         * @since 16.0.0
         */
        void setName(string name);

        /**
         * @return IResource[]
         * @since 16.0.0
         */
        IList<IResource> getResources();

        /**
         * Adds a resource to a collection
         *
         * @param IResource resource
         * @throws ResourceException when the resource is already part of the collection
         * @since 16.0.0
         */
        void addResource(IResource resource);

        /**
         * Removes a resource from a collection
         *
         * @param IResource resource
         * @since 16.0.0
         */
        void removeResource(IResource resource);

        /**
         * Can a user/guest access the collection
         *
         * @param IUser|null user
         * @return bool
         * @since 16.0.0
         */
        bool canAccess(IUser? user);
    }

}