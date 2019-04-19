using OCP.Accounts;
using Pchp.Core;
using System;
using System.Runtime.Serialization;

namespace OC.Accounts
{
    class AccountProperty : IAccountProperty
    {

    /** @var string */
    private string name;
    /** @var string */
    private string value;
    /** @var string */
    private string scope;
    /** @var string */
    private string verified;

    public AccountProperty(string name, string value, string scope, string verified)
    {
            this.name = name;
            this.value = value;
            this.scope = scope;
            this.verified = verified;
    }

        public PhpValue jsonSerialize()
        {
            var list = PhpArray.NewEmpty();
            list.Add(new IntStringKey("name"), this.getName());
            list.Add(new IntStringKey("value"), this.getValue());
            list.Add(new IntStringKey("scope"), this.getScope());
            list.Add(new IntStringKey("verified"), this.getVerified());
            return PhpValue.Create(list);
        }

        /*
         * Set the value of a property
         *
         * @since 15.0.0
         *
         * @param string $value
         * @return IAccountProperty
         */
        public IAccountProperty setValue(string value) {
            this.value = value;
            return this;
    }

        /*
         * Set the scope of a property
         *
         * @since 15.0.0
         *
         * @param string $scope
         * @return IAccountProperty
         */
        public IAccountProperty setScope(string scope) {
            this.scope = scope;
            return this;
    }

        /*
         * Set the verification status of a property
         *
         * @since 15.0.0
         *
         * @param string $verified
         * @return IAccountProperty
         */
        public IAccountProperty setVerified(string verified) {
            this.verified = verified;
            return this;
    }

    /*
     * Get the name of a property
     *
     * @since 15.0.0
     *
     * @return string
     */
    public string getName(){
            return this.name;
    }

    /*
     * Get the value of a property
     *
     * @since 15.0.0
     *
     * @return string
     */
    public string getValue() {
            return this.value;
    }

    /*
     * Get the scope of a property
     *
     * @since 15.0.0
     *
     * @return string
     */
    public string getScope() {
            return this.scope;
    }

    /*
     * Get the verification status of a property
     *
     * @since 15.0.0
     *
     * @return string
     */
    public string getVerified() {
            return this.verified;
    }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
