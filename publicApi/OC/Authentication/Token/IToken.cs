using System;
using Pchp.Core;
namespace OC.Authentication.Token
{
    public interface IToken : JsonSerializable
    {

//    const int TEMPORARY_TOKEN = 0;
//    const int PERMANENT_TOKEN = 1;
//    const int DO_NOT_REMEMBER = 0;
//    const int REMEMBER = 1;

        /**
         * Get the token ID
         *
         * @return int
         */
        int getId();

        /**
         * Get the user UID
         *
         * @return string
         */
        string getUID() ;

        /**
         * Get the login name used when generating the token
         *
         * @return string
         */
        string getLoginName() ;

    /**
     * Get the (encrypted) login password
     *
     * @return string|null
     */
    string? getPassword();

        /**
         * Get the timestamp of the last password check
         *
         * @return int
         */
        int getLastCheck() ;

    /**
     * Set the timestamp of the last password check
     *
     * @param int $time
     */
    void setLastCheck(int time);

        /**
         * Get the authentication scope for this token
         *
         * @return string
         */
        string getScope() ;

        /**
         * Get the authentication scope for this token
         *
         * @return array
         */
        PhpArray getScopeAsArray();

    /**
     * Set the authentication scope for this token
     *
     * @param array $scope
     */
    void setScope(PhpArray scope);

    /**
     * Get the name of the token
     * @return string
     */
    string getName();

    /**
     * Get the remember state of the token
     *
     * @return int
     */
    int getRemember();

    /**
     * Set the token
     *
     * @param string $token
     */
    void setToken(string token);

    /**
     * Set the password
     *
     * @param string $password
     */
    void setPassword(string password);

    /**
     * Set the expiration time of the token
     *
     * @param int|null $expires
     */
    void setExpires(int? expires);
}

}
