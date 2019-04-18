using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
         * Interface IAddressBook
         *
         * @package OCP
         * @since 5.0.0
         */
    interface IAddressBook
    {

        /**
		 * @return string defining the technical unique key
		 * @since 5.0.0
		 */
        string getKey();

        /**
		 * @return string defining the unique uri
		 * @since 16.0.0
		 * @return string
		 */
        string getUri();

		/**
		 * In comparison to getKey() this function returns a human readable (maybe translated) name
		 * @return mixed
		 * @since 5.0.0
		 */
		object getDisplayName();

        /**
		 * @param string $pattern which should match within the $searchProperties
		 * @param array $searchProperties defines the properties within the query pattern should match
		 * @param array $options Options to define the output format
		 * 	- types boolean (since 15.0.0) If set to true, fields that come with a TYPE property will be an array
		 *    example: ['id' => 5, 'FN' => 'Thomas Tanghus', 'EMAIL' => ['type => 'HOME', 'value' => 'g@h.i']]
		 * @return array an array of contacts which are arrays of key-value-pairs
		 *  example result:
		 *  [
		 *		['id' => 0, 'FN' => 'Thomas Müller', 'EMAIL' => 'a@b.c', 'GEO' => '37.386013;-122.082932'],
		 *		['id' => 5, 'FN' => 'Thomas Tanghus', 'EMAIL' => ['d@e.f', 'g@h.i']]
		 *	]
		 * @since 5.0.0
		 */
        IDictionary<string,object> search(string pattern, IDictionary<string,object> searchProperties, IDictionary<string,object> options);

        /**
		 * @param array $properties this array if key-value-pairs defines a contact
		 * @return array an array representing the contact just created or updated
		 * @since 5.0.0
		 */
        IList<object> createOrUpdate(IDictionary<string,object> properties);
        //	// dummy
        //	return array('id'    => 0, 'FN' => 'Thomas Müller', 'EMAIL' => 'a@b.c',
        //		     'PHOTO' => 'VALUE=uri:http://www.abc.com/pub/photos/jqpublic.gif',
        //		     'ADR'   => ';;123 Main Street;Any Town;CA;91921-1234'
        //	);

        /**
		 * @return mixed
		 * @since 5.0.0
		 */
        object getPermissions();

        /**
		 * @param object $id the unique identifier to a contact
		 * @return bool successful or not
		 * @since 5.0.0
		 */
        bool delete(object id);
    }
}
