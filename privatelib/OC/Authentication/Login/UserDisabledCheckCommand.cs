using OCP;

namespace OC.Authentication.Login
{
    public class UserDisabledCheckCommand : ALoginCommand
    {
        /** @var IUserManager */
        private IUserManager userManager;
        /** @var ILogger */
        private ILogger logger;

        public UserDisabledCheckCommand(IUserManager manager, ILogger logger)
        {
            this.userManager = manager;
            this.logger = logger;
        }
        public override LoginResult process(LoginData data)
        {
            var user = this.userManager.get(data.getUsername());
            if (user != null && user.isEnabled() == false)
            {
                var username = data.getUsername();
                var ip = data.getRequest().getRemoteAddress();
                logger.warning($"Login failed: {username} disabled (Remote IP: {ip}");
                return LoginResult.failure(data, LoginController::LOGIN_MSG_USERDISABLED);
            }

            return this.processNextOrFinishSuccessfully(data);
        }
    }
}