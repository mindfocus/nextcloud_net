namespace OCP.Collaboration.Resources
{
/**
 * @since 16.0.0
 */
public interface IManager : IProvider {

	/**
	 * @param int id
	 * @return ICollection
	 * @throws CollectionException when the collection could not be found
	 * @since 16.0.0
	 */
	ICollection getCollection(int id);

	/**
	 * @param int id
	 * @param IUser|null user
	 * @return ICollection
	 * @throws CollectionException when the collection could not be found
	 * @since 16.0.0
	 */
	ICollection getCollectionForUser(int id, IUser? user);

	/**
	 * @param string name
	 * @return ICollection
	 * @since 16.0.0
	 */
	ICollection newCollection(string name);

	/**
	 * Can a user/guest access the collection
	 *
	 * @param ICollection collection
	 * @param IUser|null user
	 * @return bool
	 * @since 16.0.0
	 */
	bool canAccessCollection(ICollection collection, IUser? user);

	/**
	 * @param IUser|null user
	 * @since 16.0.0
	 */
	void invalidateAccessCacheForUser(IUser? user);

	/**
	 * @param IResource resource
	 * @since 16.0.0
	 */
	void invalidateAccessCacheForResource(IResource resource);

	/**
	 * @param IResource resource
	 * @param IUser|null user
	 * @since 16.0.0
	 */
	void invalidateAccessCacheForResourceByUser(IResource resource, IUser? user) ;

	/**
	 * @param IProvider provider
	 * @since 16.0.0
	 */
	void invalidateAccessCacheForProvider(IProvider provider);

	/**
	 * @param IProvider provider
	 * @param IUser|null user
	 * @since 16.0.0
	 */
	void invalidateAccessCacheForProviderByUser(IProvider provider, IUser? user);

	/**
	 * @param string type
	 * @param string id
	 * @return IResource
	 * @since 16.0.0
	 */
	IResource createResource(string type, string id);

	/**
	 * @param string type
	 * @param string id
	 * @param IUser|null user
	 * @return IResource
	 * @throws ResourceException
	 * @since 16.0.0
	 */
	IResource getResourceForUser(string type, string id, IUser? user);

	/**
	 * @param string provider
	 * @since 16.0.0
	 */
	void registerResourceProvider(string provider);
}

}