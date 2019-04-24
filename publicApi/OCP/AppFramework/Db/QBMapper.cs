namespace OCP.AppFramework.Db
{
/// <summary>
///  Simple parent class for inheriting your data access layer from. This class
/// may be subject to change in the future
/// use Doctrine\DBAL\Exception\UniqueConstraintViolationException;
/// use OCP\DB\QueryBuilder\IQueryBuilder;
/// use OCP\IDBConnection;
/// @since 14.0.0
/// </summary>
abstract class QBMapper {

	/** @var string */
	protected tableName;

	/** @var string */
	protected entityClass;

	/** @var IDBConnection */
	protected db;

	/**
	 * @param IDBConnection db Instance of the Db abstraction layer
	 * @param string tableName the name of the table. set this to allow entity
	 * @param string entityClass the name of the entity that the sql should be
	 * mapped to queries without using sql
	 * @since 14.0.0
	 */
	public function __construct(IDBConnection db, string tableName, string entityClass=null){
		this->db = db;
		this->tableName = tableName;

		// if not given set the entity name to the class without the mapper part
		// cache it here for later use since reflection is slow
		if(entityClass === null) {
			this->entityClass = str_replace('Mapper', '', \get_class(this));
		} else {
			this->entityClass = entityClass;
		}
	}


	/**
	 * @return string the table name
	 * @since 14.0.0
	 */
	public function getTableName(): string {
		return this->tableName;
	}


	/**
	 * Deletes an entity from the table
	 * @param Entity entity the entity that should be deleted
	 * @return Entity the deleted entity
	 * @since 14.0.0
	 */
	public function delete(Entity entity): Entity {
		qb = this->db->getQueryBuilder();

		qb->delete(this->tableName)
			->where(
				qb->expr()->eq('id', qb->createNamedParameter(entity->getId()))
			);
		qb->execute();
		return entity;
	}


	/**
	 * Creates a new entry in the db from an entity
	 * @param Entity entity the entity that should be created
	 * @return Entity the saved entity with the set id
	 * @since 14.0.0
	 * @suppress SqlInjectionChecker
	 */
	public function insert(Entity entity): Entity {
		// get updated fields to save, fields have to be set using a setter to
		// be saved
		properties = entity->getUpdatedFields();

		qb = this->db->getQueryBuilder();
		qb->insert(this->tableName);

		// build the fields
		foreach(properties as property => updated) {
			column = entity->propertyToColumn(property);
			getter = 'get' . ucfirst(property);
			value = entity->getter();

			qb->setValue(column, qb->createNamedParameter(value));
		}

		qb->execute();

		if(entity->id === null) {
			entity->setId((int)qb->getLastInsertId());
		}

		return entity;
	}

	/**
	 * Tries to creates a new entry in the db from an entity and
	 * updates an existing entry if duplicate keys are detected
	 * by the database
	 *
	 * @param Entity entity the entity that should be created/updated
	 * @return Entity the saved entity with the (new) id
	 * @throws \InvalidArgumentException if entity has no id
	 * @since 15.0.0
	 * @suppress SqlInjectionChecker
	 */
	public function insertOrUpdate(Entity entity): Entity {
		try {
			return this->insert(entity);
		} catch (UniqueConstraintViolationException ex) {
			return this->update(entity);
		}
	}

	/**
	 * Updates an entry in the db from an entity
	 * @throws \InvalidArgumentException if entity has no id
	 * @param Entity entity the entity that should be created
	 * @return Entity the saved entity with the set id
	 * @since 14.0.0
	 * @suppress SqlInjectionChecker
	 */
	public function update(Entity entity): Entity {
		// if entity wasn't changed it makes no sense to run a db query
		properties = entity->getUpdatedFields();
		if(\count(properties) === 0) {
			return entity;
		}

		// entity needs an id
		id = entity->getId();
		if(id === null){
			throw new \InvalidArgumentException(
				'Entity which should be updated has no id');
		}

		// get updated fields to save, fields have to be set using a setter to
		// be saved
		// do not update the id field
		unset(properties['id']);

		qb = this->db->getQueryBuilder();
		qb->update(this->tableName);

		// build the fields
		foreach(properties as property => updated) {
			column = entity->propertyToColumn(property);
			getter = 'get' . ucfirst(property);
			value = entity->getter();

			qb->set(column, qb->createNamedParameter(value));
		}

		qb->where(
			qb->expr()->eq('id', qb->createNamedParameter(id))
		);
		qb->execute();

		return entity;
	}

	/**
	 * Returns an db result and throws exceptions when there are more or less
	 * results
	 *
	 * @see findEntity
	 *
	 * @param IQueryBuilder query
	 * @throws DoesNotExistException if the item does not exist
	 * @throws MultipleObjectsReturnedException if more than one item exist
	 * @return array the result as row
	 * @since 14.0.0
	 */
	protected function findOneQuery(IQueryBuilder query): array {
		cursor = query->execute();

		row = cursor->fetch();
		if(row === false) {
			cursor->closeCursor();
			msg = this->buildDebugMessage(
				'Did expect one result but found none when executing', query
			);
			throw new DoesNotExistException(msg);
		}

		row2 = cursor->fetch();
		cursor->closeCursor();
		if(row2 !== false ) {
			msg = this->buildDebugMessage(
				'Did not expect more than one result when executing', query
			);
			throw new MultipleObjectsReturnedException(msg);
		}

		return row;
	}

	/**
	 * @param string msg
	 * @param IQueryBuilder sql
	 * @return string
	 * @since 14.0.0
	 */
	private function buildDebugMessage(string msg, IQueryBuilder sql): string {
		return msg .
			': query "' . sql->getSQL() . '"; ';
	}


	/**
	 * Creates an entity from a row. Automatically determines the entity class
	 * from the current mapper name (MyEntityMapper -> MyEntity)
	 *
	 * @param array row the row which should be converted to an entity
	 * @return Entity the entity
	 * @since 14.0.0
	 */
	protected function mapRowToEntity(array row): Entity {
		return \call_user_func(this->entityClass .'::fromRow', row);
	}


	/**
	 * Runs a sql query and returns an array of entities
	 *
	 * @param IQueryBuilder query
	 * @return Entity[] all fetched entities
	 * @since 14.0.0
	 */
	protected function findEntities(IQueryBuilder query): array {
		cursor = query->execute();

		entities = [];

		while(row = cursor->fetch()){
			entities[] = this->mapRowToEntity(row);
		}

		cursor->closeCursor();

		return entities;
	}


	/**
	 * Returns an db result and throws exceptions when there are more or less
	 * results
	 *
	 * @param IQueryBuilder query
	 * @throws DoesNotExistException if the item does not exist
	 * @throws MultipleObjectsReturnedException if more than one item exist
	 * @return Entity the entity
	 * @since 14.0.0
	 */
	protected function findEntity(IQueryBuilder query): Entity {
		return this->mapRowToEntity(this->findOneQuery(query));
	}

}
}