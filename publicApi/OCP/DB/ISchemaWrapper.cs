using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.DB
{
    /**
     * Interface ISchemaWrapper
     *
     * @package OCP\DB
     * @since 13.0.0
     */
    public interface ISchemaWrapper
    {

        /**
         * @param string tableName
         *
         * @return \Doctrine\DBAL\Schema\Table
         * @throws \Doctrine\DBAL\Schema\SchemaException
         * @since 13.0.0
         */
        OCP.Doctrine.DBAL.Schema.Table getTable(string tableName);


        /**
         * Does this schema have a table with the given name?
         *
         * @param string tableName Prefix is automatically prepended
         *
         * @return boolean
         * @since 13.0.0
         */
        bool hasTable(string tableName);

        /**
         * Creates a new table.
         *
         * @param string tableName Prefix is automatically prepended
         * @return \Doctrine\DBAL\Schema\Table
         * @since 13.0.0
         */
        Doctrine.DBAL.Schema.Table createTable(string tableName);

        /**
         * Drops a table from the schema.
         *
         * @param string tableName Prefix is automatically prepended
         * @return \Doctrine\DBAL\Schema\Schema
         * @since 13.0.0
         */
        Doctrine.DBAL.Schema.Schema dropTable(string tableName);

        /**
         * Gets all tables of this schema.
         *
         * @return \Doctrine\DBAL\Schema\Table[]
         * @since 13.0.0
         */
        IList<Doctrine.DBAL.Schema.Table> getTables();

        /**
         * Gets all table names, prefixed with table prefix
         *
         * @return array
         * @since 13.0.0
         */
        Pchp.Core.PhpArray getTableNames();

        /**
         * Gets all table names
         *
         * @return array
         * @since 13.0.0
         */
        Pchp.Core.PhpArray getTableNamesWithoutPrefix();
    }

}
