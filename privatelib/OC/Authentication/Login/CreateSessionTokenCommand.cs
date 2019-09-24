using OC.Authentication.Token;
using OC.User;
using OCP;

namespace OC.Authentication.Login
{
    public class CreateSessionTokenCommand : ALoginCommand
    {
        private IConfig config;
        private Session userSession;

        public CreateSessionTokenCommand(IConfig config, Session session)
        {
            this.config = config;
            this.userSession = session;
        }
        public override LoginResult process(LoginData loginData)
        {
            var tokenType = RememberType.REMEMBER;
            if (this.config.getSystemValueInt("remember_login_cookie_lifetime", 60*60*24*15) == 0)
            {
                loginData.setRememberLogin(false);
                tokenType = RememberType.DO_NOT_REMEMBER;
            }
            this.userSession.createSessionToken(
                    loginData.getRequest(), loginData.getUser().getUID(), loginData.getUsername(), loginData.getPassword(), (int)tokenType
                    );
                this.userSession.updateTokens(loginData.getUser().getUID(), loginData.getPassword());
                return processNextOrFinishSuccessfully(loginData);
        }
    }
}