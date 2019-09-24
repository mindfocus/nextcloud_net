using System;
using System.Collections.Generic;
using System.Linq;
using ext;
using Microsoft.EntityFrameworkCore;
using model;
using OCP;
using OCP.Files.Cache;
using Pchp.Library.Spl;

namespace OC.Files.Cache
{
/**
 * Propagate etags and mtimes within the storage
 */
class Propagator : IPropagator {
	private bool inBatch = false;

	private IList<string> batch = new List<string>();

	/**
	 * @var \OC\Files\Storage\Storage
	 */
	protected OC.Files.Storage.Storage storage;

	/**
	 * @var IDBConnection
	 */
	private IDBConnection connection;

	/**
	 * @var array
	 */
	private IList<string> ignores = new List<string>();

	public Propagator(Files.Storage.Storage storage, IDBConnection connection, IList<string> ignores)
	{
		this.storage = storage;
		this.connection = connection;
		this.ignores = ignores;
	}

	/**
	 * @param string internalPath
	 * @param int time
	 * @param int sizeDifference number of bytes the file has grown
	 * @suppress SqlInjectionChecker
	 */
	public void propagateChange(string internalPath, int time,int sizeDifference = 0) {
		// Do not propogate changes in ignored paths
		foreach (var ignore in this.ignores) {
			if (internalPath.IndexOf(ignore, StringComparison.Ordinal) == 0)
			{
				return;
			}
//			if (strpos(internalPath, ignore) == 0) {
//				return;
//			}
		}

		var storageId = (int)this.storage.getStorageCache().getNumericId();

		var parents = this.getParents(internalPath);

		if (this.inBatch) {
			foreach (var parent in parents) {
				this.addToBatch(parent, time, sizeDifference);
			}
			return;
		}

		var parentHashes = parents.Select(o => o.MD5ext());
		var etag = Guid.NewGuid().ToString("N"); // since we give all folders the same etag we don't ask the storage for the etag
		using (var context = new NCContext())
		{
//			using (var transaction = context.Database.BeginTransaction())
//			{
//				
//			}

			var record = context.FileCaches.Single(o => o.storage == storageId && parentHashes.Contains(o.path_hash));
			record.mtime = time; // builder.createFunction('GREATEST(' . builder.getColumnName('mtime') . ', ' . builder.createNamedParameter((int)time, IQueryBuilder::PARAM_INT) . ')'))
			record.etag = etag;
			context.Update(record);
		}

		if (sizeDifference !== 0) {
			// we need to do size separably so we can ignore entries with uncalculated size
			builder = this.connection.getQueryBuilder();
			builder.update('filecache')
				.set('size', builder.func().add('size', builder.createNamedParameter(sizeDifference)))
				.where(builder.expr().eq('storage', builder.createNamedParameter(storageId, IQueryBuilder::PARAM_INT)))
				.andWhere(builder.expr().in('path_hash', hashParams))
				.andWhere(builder.expr().gt('size', builder.expr().literal(-1, IQueryBuilder::PARAM_INT)));

			builder.execute();
		}
	}

	protected IList<string> getParents(string path)
	{
		var parts = path.Split("/");
		var parent = "";
		var parents = new List<string>();
		foreach (var part in parts)
		{
			parent = parent.Trim() + "/" + part + "/";
			parents.Add(parent);
		}
		return parents;
	}

	/**
	 * Mark the beginning of a propagation batch
	 *
	 * Note that not all cache setups support propagation in which case this will be a noop
	 *
	 * Batching for cache setups that do support it has to be explicit since the cache state is not fully consistent
	 * before the batch is committed.
	 */
	public void beginBatch() {
		this.inBatch = true;
	}

	private void addToBatch(string internalPath,int time,int sizeDifference) {
		if (!isset(this.batch[internalPath])) {
			this.batch[internalPath] = [
				'hash' => md5(internalPath),
				'time' => time,
				'size' => sizeDifference
			];
		} else {
			this.batch[internalPath]['size'] += sizeDifference;
			if (time > this.batch[internalPath]['time']) {
				this.batch[internalPath]['time'] = time;
			}
		}
	}

	/**
	 * Commit the active propagation batch
	 * @suppress SqlInjectionChecker
	 */
	public void commitBatch() {
		if (!this.inBatch) {
			throw new BadMethodCallException("Not in batch");
		}
		this.inBatch = false;

		this.connection.beginTransaction();

		query = this.connection.getQueryBuilder();
		var storageId = (int)this.storage.getStorageCache().getNumericId();

		query.update('filecache')
			.set('mtime', query.createFunction('GREATEST(' . query.getColumnName('mtime') . ', ' . query.createParameter('time') . ')'))
			.set('etag', query.expr().literal(uniqid()))
			.where(query.expr().eq('storage', query.expr().literal(storageId, IQueryBuilder::PARAM_INT)))
			.andWhere(query.expr().eq('path_hash', query.createParameter('hash')));

		sizeQuery = this.connection.getQueryBuilder();
		sizeQuery.update('filecache')
			.set('size', sizeQuery.func().add('size', sizeQuery.createParameter('size')))
			.where(query.expr().eq('storage', query.expr().literal(storageId, IQueryBuilder::PARAM_INT)))
			.andWhere(query.expr().eq('path_hash', query.createParameter('hash')))
			.andWhere(sizeQuery.expr().gt('size', sizeQuery.expr().literal(-1, IQueryBuilder::PARAM_INT)));

		foreach (this.batch as item) {
			query.setParameter('time', item['time'], IQueryBuilder::PARAM_INT);
			query.setParameter('hash', item['hash']);

			query.execute();

			if (item['size']) {
				sizeQuery.setParameter('size', item['size'], IQueryBuilder::PARAM_INT);
				sizeQuery.setParameter('hash', item['hash']);

				sizeQuery.execute();
			}
		}

		this.batch = [];

		this.connection.commit();
	}


}

}