using System;
using System.Collections.Generic;
using System.Text;

namespace OC
{
    public class HintException : System.Exception
    {
        private string hint;
        /**
 * HintException constructor.
 *
 * @param string $message  The error message. It will be not revealed to the
 *                         the user (unless the hint is empty) and thus
 *                         should be not translated.
 * @param string $hint     A useful message that is presented to the end
 *                         user. It should be translated, but must not
 *                         contain sensitive data.
 * @param int $code
 * @param \Exception|null $previous
 */
        public HintException(string message, string hint = "",  int code = 0, Exception previous = null) {
            this.hint = hint;
            //parent::__construct($message, $code, $previous);
        }
        /**
 * Returns the hint with the intention to be presented to the end user. If
 * an empty hint was specified upon instatiation, the message is returned
 * instead.
 *
 * @return string
 */
        public string getHint() {
            if (this.hint == null | this.hint == "") {
                return this.Message;
            }

            return this.hint;
        }
 //       /**
 //* Returns a string representation of this Exception that includes the error
 //* code, the message and the hint.
 //*
 //* @return string
 //*/
 //       public function __toString() {
 //           return __CLASS__ . ": [{$this.code}]: {$this.message} ({$this.hint})\n";
 //       }
    }
}