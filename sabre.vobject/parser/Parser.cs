using System.Collections.Generic;

namespace sabre.vobject.parser
{
/**
 * Abstract parser.
 *
 * This class serves as a base-class for the different parsers.
 *
 * @copyright Copyright (C) fruux GmbH (https://fruux.com/)
 * @author Evert Pot (http://evertpot.com/)
 * @license http://sabre.io/license/ Modified BSD License
 */
    abstract class Parser
    {
        /**
         * Turning on this option makes the parser more forgiving.
         *
         * In the case of the MimeDir parser, this means that the parser will
         * accept slashes and underscores in property names, and it will also
         * attempt to fix Microsoft vCard 2.1's broken line folding.
         */
        const int OPTION_FORGIVING = 1;

        /**
         * If this option is turned on, any lines we cannot parse will be ignored
         * by the reader.
         */
        const int OPTION_IGNORE_INVALID_LINES = 2;

        /**
         * Bitmask of parser options.
         *
         * @var int
         */
        protected int options;

        /**
         * Creates the parser.
         *
         * Optionally, it's possible to parse the input stream here.
         *
         * @param mixed $input
         * @param int   $options any parser options (OPTION constants)
         */
        public Parser(object input = null, int options = 0)
        {
            if (input != null)
            {
                this.setInput(input);
            }

            this.options = options;
        }

        /**
         * This method starts the parsing process.
         *
         * If the input was not supplied during construction, it's possible to pass
         * it here instead.
         *
         * If either input or options are not supplied, the defaults will be used.
         *
         * @param mixed $input
         * @param int   $options
         *
         * @return array
         */
        abstract public IList<object> parse(object input = null, int options = 0);

        /**
         * Sets the input data.
         *
         * @param mixed $input
         */
        abstract public IList<object> setInput(object input);
    }
}