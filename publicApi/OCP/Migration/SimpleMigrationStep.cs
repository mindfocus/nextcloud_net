using System;
using System.Collections.Generic;
using OCP.DB;

namespace OCP.Migration
{
    /**
     * @since 13.0.0
     */
    abstract class SimpleMigrationStep : IMigrationStep
    {
    /**
     * Human readable name of the migration step
     *
     * @return string
     * @since 14.0.0
     */
    public string name()  {
        return "";
    }

/**
 * Human readable description of the migration step
 *
 * @return string
 * @since 14.0.0
 */
public string description() {
        return "";
    }

    /**
     * @param IOutput output
     * @param \Closure schemaClosure The `\Closure` returns a `ISchemaWrapper`
     * @param array options
     * @since 13.0.0
     */
    public void preSchemaChange(IOutput output, \Closure schemaClosure, array options)
{
}

/**
 * @param IOutput output
 * @param \Closure schemaClosure The `\Closure` returns a `ISchemaWrapper`
 * @param array options
 * @return null|ISchemaWrapper
 * @since 13.0.0
 */
public ISchemaWrapper changeSchema(IOutput output,Action schemaClosure, IList<string> options)
{
    return null;
}

/**
 * @param IOutput output
 * @param \Closure schemaClosure The `\Closure` returns a `ISchemaWrapper`
 * @param array options
 * @since 13.0.0
 */
public void postSchemaChange(IOutput output, Action schemaClosure, IList<string> options)
{
}
}

}
