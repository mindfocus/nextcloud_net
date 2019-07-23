using System;
using System.Collections.Generic;
using System.Text;
using OC;

namespace OCP.Federation.Exceptions
{
/**
 * Class ProviderDoesNotExistsException
 *
 * @package OCP\Federation\Exceptions
 *
 * @since 14.0.0
 */
    public class ProviderDoesNotExistsException : HintException {

    /**
     * ProviderDoesNotExistsException constructor.
     *
     * @since 14.0.0
     *
     * @param string providerId cloud federation provider ID
     */
    public ProviderDoesNotExistsException(string providerId): base("","",0,null) {
        //l = \OC::server.getL10N('federation');
        //message = 'Cloud Federation Provider with ID: "' . providerId . '" does not exist.';
        //hint = l.t('Cloud Federation Provider with ID: "%s" does not exist.', [providerId]);
        //parent::__construct(message, hint);
    }

    }

}
