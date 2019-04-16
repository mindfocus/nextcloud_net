using System;
using System.Collections.Generic;
using System.Text;

namespace privateApi.Accounts
{
    class Account : IAccount
    {

    /** @var IAccountProperty[] */
    private $properties = [];

	/** @var IUser */
	private $user;

	public function __construct(IUser $user)
    {
		$this->user = $user;
    }

    public function setProperty(string $property, string $value, string $scope, string $verified): IAccount {
		$this->properties[$property] = new AccountProperty($property, $value, $scope, $verified);
		return $this;

    }

    public function getProperty(string $property): IAccountProperty {
		if (!array_key_exists($property, $this->properties)) {
			throw new PropertyDoesNotExistException($property);
}
		return $this->properties[$property];

    }

public function getProperties(): array {
		return $this->properties;

    }

public function getFilteredProperties(string $scope = null, string $verified = null): array {
		return \array_filter($this->properties, function(IAccountProperty $obj) use($scope, $verified)
{
    if ($scope !== null && $scope !== $obj->getScope()) {
        return false;
    }
    if ($verified !== null && $verified !== $obj->getVerified()) {
        return false;
    }
    return true;
});
	}

	public function jsonSerialize()
{
    return $this->properties;
}

public function getUser(): IUser {
		return $this->user;

    }
}

}
