using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web;
using ext;
using OC.Authentication.Login;
using OC.legacy;
using OC.Security.Bruteforce;
using OC.User;
using OCP;
using OCP.AppFramework;
using OCP.AppFramework.Http;

namespace OC.core.Controllers
{
    public class LoginController : Controller
    {
        public static string LOGIN_MSG_INVALIDPASSWORD = "invalidpassword";
        public static string LOGIN_MSG_USERDISABLED = "userdisabled";

        /** @var IUserManager */
        private IUserManager userManager;

        /** @var IConfig */
        private IConfig config;

        /** @var ISession */
        private ISession session;

        /** @var IUserSession|Session */
        private IUserSession userSession;

        /** @var IURLGenerator */
        private IURLGenerator urlGenerator;

        /** @var ILogger */
        private ILogger logger;

        /** @var Defaults */
        private Defaults defaults;

        /** @var Throttler */
        private Throttler throttler;

        /** @var Chain */
        private Chain loginChain;

        /** @var IInitialStateService */
        private IInitialStateService initialStateService;

        public LoginController(string appName,
            IRequest request,
            IUserManager userManager,
            IConfig config,
            ISession session,
            IUserSession userSession,
            IURLGenerator urlGenerator,
            ILogger logger,
            Defaults defaults,
            Throttler throttler,
            Chain loginChain,
            IInitialStateService initialStateService
        ) : base(appName, request)
        {
            this.userManager = userManager;
            this.config = config;
            this.session = session;
            this.userSession = userSession;
            this.urlGenerator = urlGenerator;
            this.logger = logger;
            this.defaults = defaults;
            this.throttler = throttler;
            this.loginChain = loginChain;
            this.initialStateService = initialStateService;
        }

        /**
	 * @NoAdminRequired
	 * @UseSession
	 *
	 * @return RedirectResponse
	 */
        public RedirectResponse logout()
        {
            var loginToken = this.request.getCookie("nc_token");
            if (loginToken != null)
            {
                this.config.deleteUserValue(this.userSession.getUser().getUID(), "login_token", loginToken);
            }

            this.userSession.logout();

            var response = new RedirectResponse(this.urlGenerator.linkToRouteAbsolute(
                "core.login.showLoginForm", new Dictionary<string, object>()
                {
                    {"clear", true}
                }
                // this param the the code in login.js may be removed when the "Clear-Site-Data" is working in the browsers
            ));

            this.session.set("clearingExecutionContexts", "1");
            this.session.close();
            response.addHeader("Clear-Site-Data", "\"cache\", \"storage\"");
            return response;
        }

        /**
         * @PublicPage
         * @NoCSRFRequired
         * @UseSession
         *
         * @param string user
         * @param string redirect_url
         *
         * @return TemplateResponse|RedirectResponse
         */
        public Response showLoginForm(string user = null, string redirect_url = null)
        {
            if (this.userSession.isLoggedIn())
            {
                return new RedirectResponse(OC_Util.getDefaultPageUrl());
            }

            var loginMessages = this.session.get("loginMessages");
            if (loginMessages is IList)
            {
                var errors = ((IList) loginMessages)[0];
                var messages = ((IList) loginMessages)[1];
                this.initialStateService.provideInitialState("core", "loginMessages", messages);
                this.initialStateService.provideInitialState("core", "loginErrors", errors);
            }

            this.session.remove("loginMessages");

            if (!string.IsNullOrEmpty(user))
            {
                this.initialStateService.provideInitialState("core", "loginUsername", user);
            }
            else
            {
                this.initialStateService.provideInitialState("core", "loginUsername", "");
            }

            this.initialStateService.provideInitialState(
                "core",
                "loginAutocomplete",
                this.config.getSystemValue("login_form_autocomplete", true)
            );

            if (redirect_url.IsNotEmpty())
            {
                this.initialStateService.provideInitialState("core", "loginRedirectUrl", redirect_url);
            }

            this.initialStateService.provideInitialState(
                "core",
                "loginThrottleDelay",
                this.throttler.getDelay(this.request.getRemoteAddress())
            );

            this.setPasswordResetInitialState(user);

            // OpenGraph Support: http://ogp.me/
            Util.addHeader("meta", new Dictionary<string, object>()
            {
                {"property", "og:title"},
                {"content", Util.sanitizeHTML(this.defaults.getName())}
            });
            Util.addHeader("meta", new Dictionary<string, object>()
            {
                {"property", "og:description"},
                {"content", Util.sanitizeHTML(this.defaults.getSlogan())}
            });
            Util.addHeader("meta", new Dictionary<string, object>()
            {
                {"property", "og:site_name"},
                {"content", Util.sanitizeHTML(this.defaults.getName())}
            });
            Util.addHeader("meta", new Dictionary<string, object>()
            {
                {"property", "og:url"},
                {"content", this.urlGenerator.getAbsoluteURL("/")}
            });
            Util.addHeader("meta", new Dictionary<string, object>()
            {
                {"property", "og:type"},
                {"content", "website"}
            });
            Util.addHeader("meta", new Dictionary<string, object>()
            {
                {"property", "og:image"},
                {"content", this.urlGenerator.getAbsoluteURL(this.urlGenerator.imagePath("core", "favicon-touch.png"))}
            });


            var parameters = new Dictionary<string, object>
            {
                {"alt_login", OC_App.getAlternativeLogIns()}
            };

            return new TemplateResponse(
                this.appName, "login", parameters, "guest"
            );
        }

        /**
         * Sets the password reset state
         *
         * @param string username
         */
        private void setPasswordResetInitialState(string username)
        {
            IUser user = null;
            if (!string.IsNullOrEmpty(username))
            {
                user = this.userManager.get(username);
            }

            var passwordLink = this.config.getSystemValue("lost_password_link", "").ToString();

            this.initialStateService.provideInitialState(
                "core",
                "loginResetPasswordLink",
                passwordLink
            );

            this.initialStateService.provideInitialState(
                "core",
                "loginCanResetPassword",
                this.canResetPassword(passwordLink, user)
            );
        }

        /**
         * @param string|null passwordLink
         * @param IUser|null user
         *
         * Users may not change their passwords if:
         * - The account is disabled
         * - The backend doesn"t support password resets
         * - The password reset function is disabled
         *
         * @return bool
         */
        private bool canResetPassword(string passwordLink, IUser user)
        {
            if (passwordLink == "disabled")
            {
                return false;
            }

            if (passwordLink.IsNotEmpty() && user != null)
            {
                return user.canChangePassword();
            }

            if (user != null && user.isEnabled() == false)
            {
                return false;
            }

            return true;
        }

        private RedirectResponse generateRedirect(string redirectUrl)
        {
            if (redirectUrl != null && this.userSession.isLoggedIn())
            {
                var location = this.urlGenerator.getAbsoluteURL(HttpUtility.UrlDecode(redirectUrl));
                // Deny the redirect if the URL contains a @
                // This prevents unvalidated redirects like ?redirect_url=:user@domain.com
                if (location.IndexOf("@", StringComparison.Ordinal) == -1)
                {
                    return new RedirectResponse(location);
                }
            }

            return new RedirectResponse(OC_Util.getDefaultPageUrl());
        }

        /**
         * @PublicPage
         * @UseSession
         * @NoCSRFRequired
         * @BruteForceProtection(action=login)
         *
         * @param string user
         * @param string password
         * @param string redirect_url
         * @param string timezone
         * @param string timezone_offset
         *
         * @return RedirectResponse
         */
        public RedirectResponse tryLogin(string user,
            string password,
            string redirect_url = null,
            string timezone = "",
            string timezone_offset = "")
        {
            // If the user is already logged in and the CSRF check does not pass then
            // simply redirect the user to the correct page as required. This is the
            // case when an user has already logged-in, in another tab.
            if (!this.request.passesCSRFCheck())
            {
                return this.generateRedirect(redirect_url);
            }

            var data = new LoginData(
                this.request,
                user,
                password,
                redirect_url,
                timezone,
                timezone_offset
            );
            var result = this.loginChain.process(data);
            if (!result.isSuccess())
            {
                return this.createLoginFailedResponse(
                    data.getUsername(),
                    user,
                    redirect_url,
                    result.getErrorMessage()
                );
            }

            if (result.getRedirectUrl() != null)
            {
                return new RedirectResponse(result.getRedirectUrl());
            }

            return this.generateRedirect(redirect_url);
        }

        /**
         * Creates a login failed response.
         *
         * @param string user
         * @param string originalUser
         * @param string redirect_url
         * @param string loginMessage
         *
         * @return RedirectResponse
         */
        private RedirectResponse createLoginFailedResponse(
            string user, string originalUser, string redirect_url, string loginMessage)
        {
            // Read current user and append if possible we need to
            // return the unmodified user otherwise we will leak the login name
            var args = user != null
                ? new Dictionary<string, string>() {{"user", originalUser}}
                : new Dictionary<string, string>();
            if (redirect_url != null)
            {
                args["redirect_url"] = redirect_url;
            }

            var response = new RedirectResponse(
                this.urlGenerator.linkToRoute("core.login.showLoginForm", args)
            );
            response.throttle(new Dictionary<string, string> {{"user", user.Substring(0, 64)}});
            this.session.set("loginMessages", new List<string>
            {
                "", loginMessage, ""
            });
            return response;
        }

        /**
         * @NoAdminRequired
         * @UseSession
         * @BruteForceProtection(action=sudo)
         *
         * @param string password
         *
         * @return DataResponse
         * @license GNU AGPL version 3 or any later version
         *
         */
        public DataResponse confirmPassword(string password)
        {
            var loginName = ((Session) this.userSession).getLoginName();
            var loginResult = this.userManager.checkPassword(loginName, password);
            if (loginResult == null)
            {
                var response = new DataResponse(null, HttpStatusCode.Forbidden);
                response.throttle();
                return response;
            }

            var confirmTimestamp = DateTime.Now.ToString("R");
            this.session.set("last-password-confirm", confirmTimestamp);
            return new DataResponse(new Dictionary<string, string> {{"lastLogin", confirmTimestamp}},
                HttpStatusCode.OK);
        }
    }
}