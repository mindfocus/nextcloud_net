using OCP;

namespace OC.Authentication.Login
{
    public class UpdateLastPasswordConfirmCommand : ALoginCommand
    {
        private ISession session;

        public UpdateLastPasswordConfirmCommand(ISession session)
        {
            this.session = session;
        }
        public override LoginResult process(LoginData loginData)
        {
            this.session.set("last-password-confirm", loginData.getUser().getLastLogin());
            return this.processNextOrFinishSuccessfully(loginData);
        }
    }
}