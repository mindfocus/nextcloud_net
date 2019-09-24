using OCP;

namespace OC.Authentication.Login
{
    public class ClearLostPasswordTokensCommand : ALoginCommand
    {
        private IConfig config;

        public ClearLostPasswordTokensCommand(IConfig config)
        {
            this.config = config;
        }
        public override LoginResult process(LoginData loginData)
        {
            this.config.deleteUserValue(loginData.getUser().getUID(), "core", "lostpassword");
            return this.processNextOrFinishSuccessfully(loginData);
        }
    }
}