using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote.Api
{
    /**
     * @since 13.0.0
     */
    interface IUserApi
    {
        /**
         * @param string userId
         * @return IUser
         *
         * @since 13.0.0
         */
        IUser getUser(string userId);
    }

}
