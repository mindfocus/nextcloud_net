using OCP;

namespace OC.Authentication.Login
{
    public class EmailLoginCommand : ALoginCommand
    {
        private IUserManager userManager;

        public EmailLoginCommand(IUserManager manager)
        {
            this.userManager = manager;
        }
        public override LoginResult process(LoginData loginData)
        {
            if (loginData.getUser() == null)
            {
                var users = this.userManager.getByEmail(loginData.getUsername());
                if (users.Count == 1)
                {
                    var username = users[0].getUID();
                    if (username != loginData.getUsername())
                    {
                        var user = this.userManager.checkPassword(username, loginData.getPassword());
                        if (user != null)
                        {
                            loginData.setUser(user);
                            loginData.setUsername(username);
                        }
                    }
                }
            }
            return this.processNextOrFinishSuccessfully(loginData);
        }
    }
}