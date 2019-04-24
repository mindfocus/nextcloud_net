using System.Collections;
using System.Collections.Generic;

namespace OCP.Collaboration.Resources
{
/**
 * @since 16.0.0
 */
    public interface IResource {

        /**
         * @return string
         * @since 16.0.0
         */
        string getType();

        /**
         * @return string
         * @since 16.0.0
         */
        string getId();

        /**
         * @return string
         * @since 16.0.0
         */
        string getName();

        /**
         * @return string
         * @since 16.0.0
         */
        string getIconClass();

        /**
         * @return string
         * @since 16.0.0
         */
        string getLink();

        /**
         * Can a user/guest access the resource
         *
         * @param IUser|null user
         * @return bool
         * @since 16.0.0
         */
        bool canAccess(IUser? user);

        /**
         * @return ICollection[]
         * @since 16.0.0
         */
        IList<ICollection> getCollections();
    }
}