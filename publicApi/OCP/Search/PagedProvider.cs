using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Search
{
    /**
 * Provides a template for search functionality throughout ownCloud;
 * @since 8.0.0
 */
    abstract public class PagedProvider : Provider
    {

        /**
         * show all results
         * @since 8.0.0
         */
        int SIZE_ALL = 0;

        /**
         * Constructor
         * @param array $options
         * @since 8.0.0
         */
        public PagedProvider(IDictionary<string, object>  options) : base(options)
        {

        }

        /**
         * Search for $query
         * @param string $query
         * @return array An array of OCP\Search\Result's
         * @since 8.0.0
         */
        public override IList<Result> search(string query)
        {
            return this.searchPaged(query, 1, this.SIZE_ALL);
        }

        /**
         * Search for $query
         * @param string $query
         * @param int $page pages start at page 1
         * @param int $size 0 = SIZE_ALL
         * @return array An array of OCP\Search\Result's
         * @since 8.0.0
         */
        abstract public IList<Result> searchPaged(string query, int page, int size);
    }
}
