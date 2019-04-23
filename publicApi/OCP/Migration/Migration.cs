using System;
using System.Collections.Generic;
using OCP.DB;

namespace OCP.Migration
{
    /**
     * @since 13.0.0
     */
    public interface IMigrationStep
    {
        /**
         * Human readable name of the migration step
         *
         * @return string
         * @since 14.0.0
         */
        string name();

        /**
         * Human readable description of the migration steps
         *
         * @return string
         * @since 14.0.0
         */
        string description();

    /**
     * @param IOutput output
     * @param \Closure schemaClosure The `\Closure` returns a `ISchemaWrapper`
     * @param array options
     * @since 13.0.0
     */
    void preSchemaChange(IOutput output, Action schemaClosure, IList<string> options);

        /**
         * @param IOutput output
         * @param \Closure schemaClosure The `\Closure` returns a `ISchemaWrapper`
         * @param array options
         * @return null|ISchemaWrapper
         * @since 13.0.0
         */
        ISchemaWrapper changeSchema(IOutput output, Action schemaClosure, IList<string> options);

        /**
         * @param IOutput output
         * @param \Closure schemaClosure The `\Closure` returns a `ISchemaWrapper`
         * @param array options
         * @since 13.0.0
         */
        void postSchemaChange(IOutput output, Action schemaClosure, IList<string> options);
    }

}
