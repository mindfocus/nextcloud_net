using OC.User;

namespace OC.Authentication.Login
{
    public class UidLoginCommand : ALoginCommand
    {
        private Manager userManager;

        public UidLoginCommand(Manager manager)
        {
            this.userManager = manager;
        }
        public override LoginResult process(LoginData data)
        {
            var user = this.userManager.checkPasswordNoLogging(data.getUsername(), data.getPassword());
            data.setUser(user);
            return this.process(data);
        }
    }
}