using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Search
{
    /**
     * @since 12.0.0
     */
    public interface ISearchBinaryOperator : ISearchOperator
    {
	//const OPERATOR_AND = 'and';
	//const OPERATOR_OR = 'or';
	//const OPERATOR_NOT = 'not';

    /**
	 * The type of binary operator
	 *
	 * One of the ISearchBinaryOperator::OPERATOR_* constants
	 *
	 * @return string
	 * @since 12.0.0
	 */
    string getType();

    /**
	 * The arguments for the binary operator
	 *
	 * One argument for the 'not' operator and two for 'and' and 'or'
	 *
	 * @return ISearchOperator[]
	 * @since 12.0.0
	 */
    IList<ISearchOperator> getArguments();
}

}
