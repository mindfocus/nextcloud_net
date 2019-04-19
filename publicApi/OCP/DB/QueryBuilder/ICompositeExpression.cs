using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.DB.QueryBuilder
{
    /**
     * This class provides a wrapper around Doctrine's CompositeExpression
     * @since 8.2.0
     */
    public interface ICompositeExpression
    {
        /**
         * Adds multiple parts to composite expression.
         *
         * @param array $parts
         *
         * @return ICompositeExpression
         * @since 8.2.0
         */
        ICompositeExpression addMultiple(Pchp.Core.PhpArray parts );

        /**
         * Adds an expression to composite expression.
         *
         * @param mixed $part
         *
         * @return ICompositeExpression
         * @since 8.2.0
         */
        ICompositeExpression add(object part);

        /**
         * Retrieves the amount of expressions on composite expression.
         *
         * @return integer
         * @since 8.2.0
         */
        int count();

        /**
         * Returns the type of this composite expression (AND/OR).
         *
         * @return string
         * @since 8.2.0
         */
        string getType();
    }

}
