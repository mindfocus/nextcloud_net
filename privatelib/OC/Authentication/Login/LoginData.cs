using OCP;

namespace OC.Authentication.Login
{
    public class LoginData
    {
        /** @var IRequest */
        private IRequest request;

        /** @var string */
        private string username;

        /** @var string */
        private string password;

        /** @var string */
        private string redirectUrl;

        /** @var string */
        private string timeZone;

        /** @var string */
        private string timeZoneOffset;

        /** @var IUser|false|null */
        private IUser user = null;

        /** @var bool */
        private bool rememberLogin = true;

        public LoginData(IRequest request,
        string username,
        string password,
        string redirectUrl = null,
        string timeZone = "",
        string timeZoneOffset = "") {
            this.request = request;
                this.username = username;
                this.password = password;
                this.redirectUrl = redirectUrl;
                this.timeZone = timeZone;
                this.timeZoneOffset = timeZoneOffset;
        }

        public IRequest getRequest() {
            return this.request;
        }

        public void setUsername(string username) {
            this.username = username;
        }

        public string getUsername() {
            return this.username;
        }

        public string getPassword() {
            return this.password;
        }

        public string getRedirectUrl() {
            return this.redirectUrl;
        }

        public string getTimeZone() {
            return this.timeZone;
        }

        public string getTimeZoneOffset() {
            return this.timeZoneOffset;
        }

        /**
         * @param IUser|false|null user
         */
        public void setUser(IUser user) {
            this.user = user;
        }

        /**
         * @return false|IUser|null
         */
        public IUser getUser() {
            return this.user;
        }

        public void setRememberLogin(bool rememberLogin) {
            this.rememberLogin = rememberLogin;
        }

        public bool isRememberLogin() {
            return this.rememberLogin;
        }
    }
}