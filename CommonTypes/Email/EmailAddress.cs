namespace CommonTypes.Email
{
    public class EmailAddress
    {
        public string Name;
        public string Address;

        public EmailAddress(string address="", string name = "")
        {
            this.Name = name;
            this.Address = address;
        }
    }
}