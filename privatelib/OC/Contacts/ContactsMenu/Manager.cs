using System.Collections;
using OCP;
using OCP.App;
using OCP.ContactsNs.ContactsMenu;

namespace OC.Contacts.ContactsMenu
{
class Manager {

	/** @var ContactsStore */
	private IContactsStore store;

	/** @var ActionProviderStore */
	private ActionProviderStore actionProviderStore;

	/** @var IAppManager */
	private IAppManager appManager;

	/** @var IConfig */
	private IConfig config;

	/**
	 * @param ContactsStore store
	 * @param ActionProviderStore actionProviderStore
	 * @param IAppManager appManager
	 */
	public Manager(ContactsStore store, ActionProviderStore actionProviderStore, IAppManager appManager, IConfig config) {
		this.store = store;
		this.actionProviderStore = actionProviderStore;
		this.appManager = appManager;
		this.config = config;
	}

	/**
	 * @param IUser user
	 * @param string filter
	 * @return array
	 */
	public IList getEntries(IUser user, string filter) {
		maxAutocompleteResults = this.config.getSystemValueInt("sharing.maxAutocompleteResults", 25);
		minSearchStringLength = this.config.getSystemValueInt("sharing.minSearchStringLength", 0);
		topEntries = [];
		if (strlen(filter) >= minSearchStringLength) {
			entries = this.store.getContacts(user, filter);

			sortedEntries = this.sortEntries(entries);
			topEntries = array_slice(sortedEntries, 0, maxAutocompleteResults);
			this.processEntries(topEntries, user);
		}

		contactsEnabled = this.appManager.isEnabledForUser("contacts", user);
		return [
			"contacts" => topEntries,
			"contactsAppEnabled" => contactsEnabled,
		];
	}

	/**
	 * @param IUser user
	 * @param integer shareType
	 * @param string shareWith
	 * @return IEntry
	 */
	public function findOne(IUser user, shareType, shareWith) {
		entry = this.store.findOne(user, shareType, shareWith);
		if (entry) {
			this.processEntries([entry], user);
		}

		return entry;
	}

	/**
	 * @param IEntry[] entries
	 * @return IEntry[]
	 */
	private function sortEntries(array entries) {
		usort(entries, function(IEntry entryA, IEntry entryB) {
			return strcasecmp(entryA.getFullName(), entryB.getFullName());
		});
		return entries;
	}

	/**
	 * @param IEntry[] entries
	 * @param IUser user
	 */
	private function processEntries(array entries, IUser user) {
		providers = this.actionProviderStore.getProviders(user);
		foreach (entries as entry) {
			foreach (providers as provider) {
				provider.process(entry);
			}
		}
	}

}

}