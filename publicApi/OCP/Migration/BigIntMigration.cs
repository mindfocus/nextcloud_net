using System;
namespace OCP.Migration
{
    /**
     * @since 13.0.0
     */
    abstract class BigIntMigration : SimpleMigrationStep
    {

    /**
     * @return array Returns an array with the following structure
     * ['table1' => ['column1', 'column2'], ...]
     * @since 13.0.0
     */
    abstract protected function getColumnsByTable();

    /**
     * @param IOutput output
     * @param \Closure schemaClosure The `\Closure` returns a `ISchemaWrapper`
     * @param array options
     * @return null|ISchemaWrapper
     * @since 13.0.0
     */
    public function changeSchema(IOutput output, \Closure schemaClosure, array options)
    {
        /** @var ISchemaWrapper schema */
        schema = schemaClosure();

        tables = this->getColumnsByTable();

        foreach (tables as tableName => columns) {
            table = schema->getTable(tableName);

            foreach (columns as columnName)
            {
                column = table->getColumn(columnName);
                if (column->getType()->getName() !== Type::BIGINT)
                {
                    column->setType(Type::getType(Type::BIGINT));
                    column->setOptions(['length' => 20]);
                }
            }
        }

        return schema;
    }
}

}
