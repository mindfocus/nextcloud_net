using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OCP;
using OCP.ContactsNs;

namespace OC
{
    class ContactsManager : IManager
    {
        /**
     * @var \Closure[] to call to load/register address books
     */
        private IList<Action<IManager>> addressBookLoaders = new List<Action<IManager>>();

        /**
         * @var \OCP\IAddressBook[] which holds all registered address books
         */
        private IDictionary<string, IAddressBook> addressBooks = new Dictionary<string, IAddressBook>();

        /**
         * This function is used to search and find contacts within the users address books.
         * In case $pattern is empty all contacts will be returned.
         *
         * @param string $pattern which should match within the $searchProperties
         * @param array $searchProperties defines the properties within the query pattern should match
         * @param array $options - for future use. One should always have options!
         * @return array an array of contacts which are arrays of key-value-pairs
         */
        public IList<IDictionary<string, object>> search(string pattern, IDictionary<string, object> searchProperties,
            IDictionary<string, object> options)
        {
            this.loadAddressBooks();
            var result = new List<IDictionary<string, object>>();
            foreach (var addressBook in this.addressBooks)
            {
                var r = addressBook.Value.search(pattern, searchProperties, options);
                var contacts = new List<IDictionary<string, object>>();
                foreach (var c in r)
                {
                    c["addressbook-key"] = addressBook.Value.getKey();
                    contacts.Add(c);
                }

                result.AddRange(contacts);
            }

            return result;
        }

        /**
         * This function can be used to delete the contact identified by the given id
         *
         * @param object $id the unique identifier to a contact
         * @param string $addressBookKey identifier of the address book in which the contact shall be deleted
         * @return bool successful or not
         */
        public bool delete(string id, string addressBookKey)
        {
            var addressBook = this.getAddressBook(addressBookKey);
            if (addressBook == null) {
                return true;
            }

            if ((addressBook.getPermissions() & Constants.PERMISSION_DELETE) == 1) {
                return addressBook.delete(id);
            }
            return true;
        }

        /**
         * This function is used to create a new contact if 'id' is not given or not present.
         * Otherwise the contact will be updated by replacing the entire data set.
         *
         * @param array $properties this array if key-value-pairs defines a contact
         * @param string $addressBookKey identifier of the address book in which the contact shall be created or updated
         * @return array representing the contact just created or updated
         */
        public IList<object> createOrUpdate(IDictionary<string,object> properties, string addressBookKey) {
            var addressBook = this.getAddressBook(addressBookKey);
            if (addressBook == null)
            {
                return null;
            }

            // TODO @focus 位移操作结果待确认
            if ((addressBook.getPermissions() & OCP.Constants.PERMISSION_CREATE) == 0)
            {
                return addressBook.createOrUpdate(properties);
            }

            return null;
        }

        /**
         * Check if contacts are available (e.g. contacts app enabled)
         *
         * @return bool true if enabled, false if not
         */
        public bool isEnabled()
        {
            return this.addressBooks.Count != 0 || this.addressBookLoaders.Count != 0;
        }

        /**
         * @param \OCP\IAddressBook $addressBook
         */
        public void registerAddressBook(IAddressBook addressBook) {
            this.addressBooks.Add(addressBook.getKey(), addressBook);
        }

        /**
         * @param \OCP\IAddressBook $addressBook
         */
        public void unregisterAddressBook(IAddressBook addressBook)
        {
            this.addressBooks.Remove(addressBook.getKey());
        }

        /**
         * Return a list of the user's addressbooks display names
         * ! The addressBook displayName are not unique, please use getUserAddressBooks
         * 
         * @return array
         * @since 6.0.0
         * @deprecated 16.0.0 - Use `$this->getUserAddressBooks()` instead
         */
        public IDictionary<string, string> getAddressBooks()
        {
            this.loadAddressBooks();
            var result = new Dictionary<string,string>();
            foreach (var addressBook in this.addressBooks)
            {
                result.Add(addressBook.Value.getKey(), addressBook.Value.getDisplayName());
               
            }
            return result;
        }

        /**
         * Return a list of the user's addressbooks
         * 
         * @return IAddressBook[]
         * @since 16.0.0
         */
        public IDictionary<string, IAddressBook> getUserAddressBooks() {
            this.loadAddressBooks();
            return this.addressBooks;
        }

        /**
         * removes all registered address book instances
         */
        public void clear()
        {
            this.addressBooks.Clear();
            this.addressBookLoaders.Clear();
        }


        /**
         * In order to improve lazy loading a closure can be registered which will be called in case
         * address books are actually requested
         *
         * @param \Closure $callable
         */
        public void register(Action<IManager> callable)
        {
            this.addressBookLoaders.Add(callable);
        }

        /**
         * Get (and load when needed) the address book for $key
         *
         * @param string $addressBookKey
         * @return \OCP\IAddressBook
         */
        protected IAddressBook getAddressBook(string addressBookKey)
        {
            this.loadAddressBooks();
            if (this.addressBooks.ContainsKey(addressBookKey))
            {
                return this.addressBooks[addressBookKey];
            }

            return null;
        }

        /**
         * Load all address books registered with 'register'
         */
        protected void loadAddressBooks()
        {
            foreach (var loader in this.addressBookLoaders)
            {
                loader(this);
            }

            this.addressBookLoaders.Clear();
        }
    }
}