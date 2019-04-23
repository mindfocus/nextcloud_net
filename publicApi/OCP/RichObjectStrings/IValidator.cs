using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.RichObjectStrings
{
    /**
     * Class Validator
     *
     * @package OCP\RichObjectStrings
     * @since 11.0.0
     */
    public interface IValidator
    {

        /**
         * @param string subject
         * @param array[] parameters
         * @throws InvalidObjectExeption
         * @since 11.0.0
         */
        void validate(string subject, IList<string> parameters);
    }

}
