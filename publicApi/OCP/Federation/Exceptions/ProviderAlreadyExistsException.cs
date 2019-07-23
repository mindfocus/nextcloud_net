using System;
using System.Collections.Generic;
using System.Text;
using OC;

namespace OCP.Federation.Exceptions
{
/**
 * Class ProviderAlreadyExistsException
 *
 * @package OCP\Federation\Exceptions
 *
 * @since 14.0.0
 */
    public class ProviderAlreadyExistsException : HintException {

    /**
     * ProviderAlreadyExistsException constructor.
     *
     * @since 14.0.0
     *
     * @param string newProviderId cloud federation provider ID of the new provider
     * @param string existingProviderName name of cloud federation provider which already use the same ID
     */
    public ProviderAlreadyExistsException(string newProviderId, string existingProviderName) {
        //l = \OC::server.getL10N('federation');
        //message = 'ID "' . newProviderId . '" already used by cloud federation provider "' . existingProviderName . '"';
        //hint = l.t('ID "%1s" already used by cloud federation provider "%2s"', [newProviderId, existingProviderName]);
        //parent::__construct(message, hint);
    }

    }

}
