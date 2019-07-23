using System;
using System.Collections.Generic;
using System.Text;
using OC;

namespace OCP.Federation.Exceptions
{
/**
 * Class ActionNotSupportedException
 *
 * @package OCP\Federation\Exceptions
 *
 * @since 14.0.0
 */
    public class ActionNotSupportedException : HintException {

    /**
     * ActionNotSupportedException constructor.
     *
     * @since 14.0.0
     *
     */
    public ActionNotSupportedException(string action) {
        //l = \OC::server.getL10N('federation');
        //message = 'Action "' . action . '" not supported or implemented.';
        //hint = l.t('Action "%s" not supported or implemented.', [action]);
        //parent::__construct(message, hint);
    }

    }

}
