using OCP.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace privateApi.Accounts
{
    public class Account : IAccount
    {

    /** @var IAccountProperty[] */
    private IList<IAccountProperty> properties = new List<IAccountProperty>();

	/** @var IUser */
	private IUser user;

	public Account(IUser user)
    {
            this.user = user;
    }

    public IAccount setProperty(string property, string value, string scope, string verified) {
		this.properties[property] = new AccountProperty(property, value, scope, verified);
		return this;

    }

    public IAccountProperty getProperty(string property)  {
		if (!array_key_exists(property, this.properties)) {
			throw new PropertyDoesNotExistException(property);
}
		return this.properties[property];

    }

public IList<IAccountProperty> getProperties() {
		return this.properties;

    }

public function getFilteredProperties(string scope = null, string verified = null): array {
		return \array_filter(this.properties, function(IAccountProperty obj) use(scope, verified)
{
    if (scope !== null && scope !== obj.getScope()) {
        return false;
    }
    if (verified !== null && verified !== obj.getVerified()) {
        return false;
    }
    return true;
});
	}

	public function jsonSerialize()
{
    return this.properties;
}

public function getUser(): IUser {
		return this.user;

    }
}

}
