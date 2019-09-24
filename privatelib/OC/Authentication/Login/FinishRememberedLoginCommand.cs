using OC.User;

namespace OC.Authentication.Login
{
    public class FinishRememberedLoginCommand : ALoginCommand
    {
        private Session userSession;

        public FinishRememberedLoginCommand(Session userSession)
        {
            this.userSession = userSession;
        }

        public override LoginResult process(LoginData loginData)
        {
            if (loginData.isRememberLogin())
            {
                this.userSession.createRememberMeToken(loginData.getUser());
            }

            return this.processNextOrFinishSuccessfully(loginData);
        }
    }
}