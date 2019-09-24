namespace OC.Authentication.Login
{
    public  abstract class ALoginCommand
    {
        protected ALoginCommand next;

        public ALoginCommand setNext(ALoginCommand next)
        {
            this.next = next;
            return next;
        }

        protected LoginResult processNextOrFinishSuccessfully(LoginData loginData)
        {
            if (this.next != null)
            {
                return this.next.process(loginData);
            }
            else
            {
                return LoginResult.success(loginData);
            }
        }

        public abstract LoginResult process(LoginData loginData);

    }
}