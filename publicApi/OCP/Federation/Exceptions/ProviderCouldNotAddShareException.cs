using System;
using System.Collections.Generic;
using System.Text;
using OC;

namespace OCP.Federation.Exceptions
{
/**
 * Class ProviderCouldNotAddShareException
 *
 * @package OCP\Federation\Exceptions
 *
 * @since 14.0.0
 */
    public class ProviderCouldNotAddShareException : HintException {

    /**
     * ProviderCouldNotAddShareException constructor.
     *
     * @since 14.0.0
     *
     * @param string message
     * @param string hint
     * @param int code
     * @param \Exception|null previous
     */
    public ProviderCouldNotAddShareException(string message, string hint = "", int code = Http.STATUS_BAD_REQUEST, Exception previous = null) 
    : base(message,hint,code,previous){
        //parent::__construct(message, hint, code, previous);
    }


    }

}
