using System;
using System.Collections.Generic;
using System.Text;
using OC;

namespace OCP.Federation.Exceptions
{
/**
 * Class BadRequestException
 *
 * @package OCP\Federation\Exceptions
 *
 * @since 14.0.0
 */
    class BadRequestException : HintException {

    private IList<string> parameterList;

    /**
     * BadRequestException constructor.
     *
     * @since 14.0.0
     *
     * @param array missingParameters
     */
    public BadRequestException(IList<string> missingParameters) {
        //l = \OC::server.getL10N('federation');
        //this.parameterList = missingParameters;
        //parameterList = implode(',', missingParameters);
        //message = 'Parameters missing in order to complete the request. Missing Parameters: ' . parameterList;
        //hint = l.t('Parameters missing in order to complete the request. Missing Parameters: "%s"', [parameterList]);
        //parent::__construct(message, hint);
    }

    /**
     * get array with the return message as defined in the OCM API
     *
     * @since 14.0.0
     *
     * @return array
     */
    public IList<string> getReturnMessage() {
        //result = [
        //'message' => 'RESOURCE_NOT_FOUND',
        //'validationErrors' =>[
        //    ]
        //    ];

        //foreach (this.parameterList as missingParameter) {
        //    result['validationErrors'] = [
        //    'name' => missingParameter,
        //    'message' => 'NOT_FOUND'
        //        ];
        //}

        //return result;
        return null;
    }

    }

}
