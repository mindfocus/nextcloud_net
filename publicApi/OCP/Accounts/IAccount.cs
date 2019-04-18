using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OCP.Accounts
{
    public interface IAccount : ISerializable
    {
        /// <summary>
        /// Set a property with data
        /// </summary>
        /// <param name="property">Must be one of the PROPERTY_ prefixed constants of \OCP\Accounts\IAccountManager</param>
        /// <param name="value"></param>
        /// <param name="scope"></param>
        /// <param name="verified"></param>
        /// <returns></returns>
        IAccount setProperty(string property, string value, string
            scope, string verified);

        /// <summary>
        /// Get a property by its key
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IAccountProperty getProperty(string property);

        /// <summary>
        /// Get all properties of an account
        /// </summary>
        /// <returns></returns>
        List<IAccountProperty> getProperties();

        /// <summary>
        /// Get all properties that match the provided filters for scope and verification status
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="verified"></param>
        /// <returns></returns>
        List<IAccountProperty> getFilteredProperties(string scope = "", string verified = "");

        /// <summary>
        /// Get the related user for the account data
        /// </summary>
        /// <returns></returns>
        IUser getUser();
    }
}