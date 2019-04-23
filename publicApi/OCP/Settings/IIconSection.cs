using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Settings
{
    /**
     * @since 12
     */
    interface IIconSection : ISection
    {
    /**
	 * returns the relative path to an 16*16 icon describing the section.
	 * e.g. '/core/img/places/files.svg'
	 *
	 * @returns string
	 * @since 12
	 */
    string getIcon();
}

}
