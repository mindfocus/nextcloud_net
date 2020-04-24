using OCP;

namespace OC.Authentication.Login
{
    public class LoggedInCheckCommand : ALoginCommand
    {
        private ILogger logger;

        public LoggedInCheckCommand(ILogger logger)
        {
            this.logger = logger;
        }
        public override LoginResult process(LoginData loginData)
        {
            if (loginData.getUser() == null)
            {
                var username = loginData.getUsername();
                var ip = loginData.getRequest().getRemoteAddress();
                this.logger.warning($"Login failed: {username} (Remote IP: {ip})");
                return LoginResult.failure(loginData, "invalidpassword");
            }

            return this.processNextOrFinishSuccessfully(loginData);
        }
    }
}