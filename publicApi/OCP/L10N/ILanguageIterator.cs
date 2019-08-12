using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.L10N
{
    /**
 * Interface ILanguageIterator
 *
 * iterator across language settings (if provided) in this order:
 * 1. returns the forced language or:
 * 2. if applicable, the trunk of 1 (e.g. "fu" instead of "fu_BAR"
 * 3. returns the user language or:
 * 4. if applicable, the trunk of 3
 * 5. returns the system default language or:
 * 6. if applicable, the trunk of 5
 * 7+∞. returns 'en'
 *
 * if settings are not present or truncating is not applicable, the iterator
 * skips to the next valid item itself
 *
 * @package OCP\L10N
 *
 * @since 14.0.0
 */
public interface ILanguageIterator : Iterator {

	/**
	 * Return the current element
	 *
	 * @since 14.0.0
	 */
	string current() ;

	/**
	 * Move forward to next element
	 *
	 * @since 14.0.0
	 */
	void next();

	/**
	 * Return the key of the current element
	 *
	 * @since 14.0.0
	 */
	int key();

	/**
	 * Checks if current position is valid
	 *
	 * @since 14.0.0
	 */
	bool valid();
}

}