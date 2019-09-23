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

        protected LoginResult processNextOrFinishSuccessfully(LoginData data)
        {
            if (this.next != null)
            {
                return this.next.process(data);
            }
            else
            {
                return LoginResult.success(data);
            }
        }

        public abstract LoginResult process(LoginData data);

    }
}