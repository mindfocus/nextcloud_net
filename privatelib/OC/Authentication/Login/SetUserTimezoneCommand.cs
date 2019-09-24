using ext;
using OCP;

namespace OC.Authentication.Login
{
    public class SetUserTimezoneCommand : ALoginCommand
    {
        private IConfig config;
        private ISession session;

        public SetUserTimezoneCommand(IConfig config, ISession session)
        {
            this.config = config;
            this.session = session;
        }
        public override LoginResult process(LoginData loginData)
        {
            if(loginData.getTimeZoneOffset().IsNotEmpty())
            {
                this.config.setUserValue(loginData.getUser().getUID(), "core" , "timezone", loginData.getTimeZone());
                this.session.set("timezone", loginData.getTimeZoneOffset());
            }
            return this.processNextOrFinishSuccessfully(loginData);
        }
    }
}