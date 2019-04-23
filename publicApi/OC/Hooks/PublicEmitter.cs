using System;
using System.Collections.Generic;
using System.Text;

namespace OC.Hooks
{
    class PublicEmitter : BasicEmitter
    {
    /**
	 * @param string $scope
	 * @param string $method
	 * @param array $arguments optional
	 *
	 * @suppress PhanAccessMethodProtected
	 */
    public void emit(string scope, string method, IList<string> arguments )
    {
            base.emit(scope, method, arguments);
    }
}
}
