using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote
{
    /**
     * @since 13.0.0
     */
    public interface IInstanceFactory
    {
        /**
         * @param string $url
         * @return IInstance
         *
         * @since 13.0.0
         */
        IInstance getInstance(string url);
    }
}
