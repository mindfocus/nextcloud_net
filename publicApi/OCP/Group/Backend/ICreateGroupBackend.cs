﻿using System;
namespace OCP.Group.Backend
{
    /**
     * @since 14.0.0
     */
    public interface ICreateGroupBackend : IBackend
    {

        /**
         * @since 14.0.0
         */
        bool createGroup(string gid) ;
}

}
