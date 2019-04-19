using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.DB.QueryBuilder
{
    /**
     * This class provides a builder for sql some functions
     *
     * @since 12.0.0
     */
    public interface IFunctionBuilder
    {
        /**
         * Calculates the MD5 hash of a given input
         *
         * @param mixed $input The input to be hashed
         *
         * @return IQueryFunction
         * @since 12.0.0
         */
        IQueryFunction md5(object input);

        /**
         * Combines two input strings
         *
         * @param mixed $x The first input string
         * @param mixed $y The seccond input string
         *
         * @return IQueryFunction
         * @since 12.0.0
         */
        IQueryFunction concat(object x, object y);

        /**
         * Takes a substring from the input string
         *
         * @param mixed $input The input string
         * @param mixed $start The start of the substring, note that counting starts at 1
         * @param mixed $length The length of the substring
         *
         * @return IQueryFunction
         * @since 12.0.0
         */
        IQueryFunction substring(object input, object start, object length = null);

        /**
         * Takes the sum of all rows in a column
         *
         * @param mixed $field the column to sum
         *
         * @return IQueryFunction
         * @since 12.0.0
         */
        IQueryFunction sum(object field);

        /**
         * Transforms a string field or value to lower case
         *
         * @param mixed $field
         * @return IQueryFunction
         * @since 14.0.0
         */
        IQueryFunction lower(object field);

        /**
         * @param mixed $x The first input field or number
         * @param mixed $y The second input field or number
         * @return IQueryFunction
         * @since 14.0.0
         */
        IQueryFunction add(object x, object y);

        /**
         * @param mixed $x The first input field or number
         * @param mixed $y The second input field or number
         * @return IQueryFunction
         * @since 14.0.0
         */
        IQueryFunction subtract(object x, object y);

        /**
         * @param mixed $count The input to be counted
         * @param string $alias Alias for the counter
         *
         * @return IQueryFunction
         * @since 14.0.0
         */
        IQueryFunction count(object count, string alias = "");
    }

}
