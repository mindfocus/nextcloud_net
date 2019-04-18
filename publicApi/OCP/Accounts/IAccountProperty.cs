using System;
using System.Runtime.Serialization;

namespace OCP.Accounts
{
    public interface IAccountProperty : ISerializable
    {
        /// <summary>
        /// Set the value of a property
        /// @since 15.0.0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IAccountProperty setValue(string value);

        /// <summary>
        /// Set the scope of a property
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        IAccountProperty setScope(string scope);

        /// <summary>
        /// Set the verification status of a property
        /// </summary>
        /// <param name="verified"></param>
        /// <returns></returns>
        IAccountProperty setVerified(string verified);

        /// <summary>
        /// Get the name of a property
        /// </summary>
        /// <returns></returns>
        string getName();

        /// <summary>
        /// Get the value of a property
        /// </summary>
        /// <returns></returns>
        string getValue();

        /// <summary>
        /// Get the scope of a property
        /// </summary>
        /// <returns></returns>
        string getScope();

        /// <summary>
        /// Get the verification status of a property
        /// </summary>
        /// <returns></returns>
        string getVerified();
    }
}