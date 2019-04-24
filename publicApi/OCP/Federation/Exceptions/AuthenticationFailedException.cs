using System;
using System.Collections.Generic;
using System.Text;
using OC;

namespace OCP.Federation.Exceptions
{
/**
 * Class AuthenticationFailedException
 *
 * @package OCP\Federation\Exceptions
 *
 * @since 14.0.0
 */
    public class AuthenticationFailedException : HintException {

    /**
     * BadRequestException constructor.
     *
     * @since 14.0.0
     *
     */
    public AuthenticationFailedException() :base("",""){
        //l = \OC::server->getL10N('federation');
        //message = 'Authentication failed, wrong token or provider ID given';
        //hint = l->t('Authentication failed, wrong token or provider ID given');
        //parent::__construct(message, hint);
    }

    }

}
