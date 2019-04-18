using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Search
{
    /**
     * @since 12.0.0
     */
    public interface ISearchQuery
    {
        /**
         * @return ISearchOperator
         * @since 12.0.0
         */
        ISearchOperator getSearchOperation();

        /**
         * Get the maximum number of results to return
         *
         * @return integer
         * @since 12.0.0
         */
        int getLimit();

        /**
         * Get the offset for returned results
         *
         * @return integer
         * @since 12.0.0
         */
        int getOffset();

        /**
         * The fields and directions to order by
         *
         * @return ISearchOrder[]
         * @since 12.0.0
         */
        IList<ISearchOrder> getOrder();

        /**
         * The user that issued the search
         *
         * @return IUser
         * @since 12.0.0
         */
        IUser getUser();
    }

}
