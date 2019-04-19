using OCP;
using OCP.Accounts;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OC.Accounts
{
    public class Account : IAccount
    {

        /** @var IAccountProperty[] */
        private IDictionary<string, IAccountProperty> properties = new Dictionary<string, IAccountProperty>();

        /** @var IUser */
        private IUser user;

        public Account(IUser user)
        {
            this.user = user;
        }

        public IAccount setProperty(string property, string value, string scope, string verified)
        {
            this.properties[property] = new AccountProperty(property, value, scope, verified);
            return this;

        }

        public IAccountProperty getProperty(string property)
        {
            if (properties.ContainsKey(property))
            {
                return properties[property];
            }
            throw new Exception();
            //		if (!Pchp.Library.Arrays.array_key_exists(property, this.properties)) {
            //			throw new PropertyDoesNotExistException(property);
            //}
            //		return this.properties[property];

        }

        public IDictionary<string, IAccountProperty> getProperties()
        {
            return this.properties;

        }

        public IDictionary<string, IAccountProperty> getFilteredProperties(string scope = null, string verified = null)
        {
            return this.properties.Where(o =>
               o.Value.getScope() == scope && o.Value.getVerified() == verified
           ).ToDictionary(p => p.Key, p => p.Value);

        }

        public IUser getUser()
        {
            return this.user;

        }
    }

}
