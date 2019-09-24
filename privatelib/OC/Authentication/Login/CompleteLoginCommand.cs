using System.Collections.Generic;
using OC.User;

namespace OC.Authentication.Login
{
    public class CompleteLoginCommand : ALoginCommand
    {
        /** @var Session */
        private OC.User.Session userSession;

        public CompleteLoginCommand(Session session)
        {
            userSession = session;
        }
        public override LoginResult process(LoginData loginData)
        {
            this.userSession.completeLogin(loginData.getUser(), new Dictionary<string, string>()
                {
                    {"loginName", loginData.getUsername()},
                    {"password" , loginData.getPassword()}
                });
                return this.processNextOrFinishSuccessfully(loginData);
        }
    }
}