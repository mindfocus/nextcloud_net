using OC.Hooks;
using OCP;

namespace OC.Authentication.Login
{
    public class PreLoginHookCommand : ALoginCommand
    {
        private IUserManager userManager;

        public PreLoginHookCommand(IUserManager manager)
        {
            this.userManager = manager;
        }

        public override LoginResult process(LoginData data)
        {
            if (this.userManager is PublicEmitter)
            {
//                $this->userManager->emit(
//                    '\OC\User',
//                'preLogin',
//                    [
//                    $loginData->getUsername(),
//                    $loginData->getPassword(),
//                    ]
//                    );
            }

            return this.processNextOrFinishSuccessfully(data);
        }
    }
}