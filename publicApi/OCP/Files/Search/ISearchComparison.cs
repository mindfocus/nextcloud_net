using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Search
{
    /**
     * @since 12.0.0
     */
    public interface ISearchComparison : ISearchOperator
    {
	//const COMPARE_EQUAL = 'eq';
	//const COMPARE_GREATER_THAN = 'gt';
	//const COMPARE_GREATER_THAN_EQUAL = 'gte';
	//const COMPARE_LESS_THAN = 'lt';
	//const COMPARE_LESS_THAN_EQUAL = 'lte';
	//const COMPARE_LIKE = 'like';

    /**
	 * Get the type of comparison, one of the ISearchComparison::COMPARE_* constants
	 *
	 * @return string
	 * @since 12.0.0
	 */
    string  getType();

    /**
	 * Get the name of the field to compare with
	 *
	 * i.e. 'size', 'name' or 'mimetype'
	 *
	 * @return string
	 * @since 12.0.0
	 */
    string  getField();

    /**
	 * Get the value to compare the field with
	 *
	 * @return string|integer|\DateTime
	 * @since 12.0.0
	 */
    object getValue();
}

}
