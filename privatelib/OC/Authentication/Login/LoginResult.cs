namespace OC.Authentication.Login
{
    public class LoginResult
    {
        /** @var bool */
        private bool _success;

        /** @var LoginData */
        private LoginData loginData;

        /** @var string|null */
        private string _redirectUrl;

        /** @var string|null */
        private string errorMessage;

        private LoginResult(bool success, LoginData loginData) {
            this._success = success;
                this.loginData = loginData;
        }

        private void setRedirectUrl(string url) {
            this._redirectUrl = url;
        }

        private void setErrorMessage(string msg) {
            this.errorMessage = msg;
        }

        public static LoginResult success(LoginData data, string redirectUrl = null) {
            var result = new LoginResult(true, data);
            if (redirectUrl != null) {
                result.setRedirectUrl(redirectUrl);
            }
            return result;
        }

        public static LoginResult failure(LoginData data, string msg = null) {
            var result = new LoginResult(false, data);
            if (msg != null) {
                result.setErrorMessage(msg);
            }
            return result;
        }

        public bool isSuccess() {
            return this._success;
        }

        public string? getRedirectUrl(){
            return this._redirectUrl;
        }

        public string? getErrorMessage() {
            return this.errorMessage;
        }
    }
}