using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Search
{
    /**
     * @since 12.0.0
     */
    public interface ISearchOrder
    {
        //const DIRECTION_ASCENDING = 'asc';
        //const DIRECTION_DESCENDING = 'desc';

        /**
         * The direction to sort in, either ISearchOrder::DIRECTION_ASCENDING or ISearchOrder::DIRECTION_DESCENDING
         *
         * @return string
         * @since 12.0.0
         */
        string getDirection();

        /**
         * The field to sort on
         *
         * @return string
         * @since 12.0.0
         */
        string getField();
    }

}
