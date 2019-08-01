using System.Collections.Generic;

namespace OC.Authentication.Token
{
    public interface IProvider
    {
        /**
	        * Create and persist a new token
	        *
	        * @param string token
	        * @param string uid
	        * @param string loginName
	        * @param string|null password
	        * @param string name
	        * @param int type token type
	        * @param int remember whether the session token should be used for remember-me
	        * @return IToken
	        * @throws \RuntimeException when OpenSSL reports a problem
	        */
        IToken generateToken(string token,
            string uid,
            string loginName,
            string? password,
            string name,
            TokenType type = TokenType.PERMANENT_TOKEN,
            RememberType remember = RememberType.DO_NOT_REMEMBER);

        /**
         * Get a token by token id
         *
         * @param string tokenId
         * @throws InvalidTokenException
         * @throws ExpiredTokenException
         * @throws WipeTokenException
         * @return IToken
         */
        IToken getToken(string tokenId);

        /**
         * Get a token by token id
         *
         * @param int tokenId
         * @throws InvalidTokenException
         * @throws ExpiredTokenException
         * @throws WipeTokenException
         * @return IToken
         */
        IToken getTokenById(int tokenId);

        /**
         * Duplicate an existing session token
         *
         * @param string oldSessionId
         * @param string sessionId
         * @throws InvalidTokenException
         * @throws \RuntimeException when OpenSSL reports a problem
         */
        void renewSessionToken(string oldSessionId, string sessionId);

        /**
         * Invalidate (delete) the given session token
         *
         * @param string token
         */
        void invalidateToken(string token);

        /**
         * Invalidate (delete) the given token
         *
         * @param string uid
         * @param int id
         */
        void invalidateTokenById(string uid, int id);

        /**
         * Invalidate (delete) old session tokens
         */
        void invalidateOldTokens();

        /**
         * Save the updated token
         *
         * @param IToken token
         */
        void updateToken(IToken token);

        /**
         * Update token activity timestamp
         *
         * @param IToken token
         */
        void updateTokenActivity(IToken token);

        /**
         * Get all tokens of a user
         *
         * The provider may limit the number of result rows in case of an abuse
         * where a high number of (session) tokens is generated
         *
         * @param string uid
         * @return IToken[]
         */
        IList<IToken> getTokenByUser(string uid);

        /**
         * Get the (unencrypted) password of the given token
         *
         * @param IToken token
         * @param string tokenId
         * @throws InvalidTokenException
         * @throws PasswordlessTokenException
         * @return string
         */
        string getPassword(IToken token, string tokenId);

        /**
         * Encrypt and set the password of the given token
         *
         * @param IToken token
         * @param string tokenId
         * @param string password
         * @throws InvalidTokenException
         */
        void setPassword(IToken token, string tokenId, string password);

        /**
         * Rotate the token. Usefull for for example oauth tokens
         *
         * @param IToken token
         * @param string oldTokenId
         * @param string newTokenId
         * @return IToken
         * @throws \RuntimeException when OpenSSL reports a problem
         */
        IToken rotate(IToken token, string oldTokenId, string newTokenId);

        /**
         * Marks a token as having an invalid password.
         *
         * @param IToken token
         * @param string tokenId
         */
        void markPasswordInvalid(IToken token, string tokenId);

        /**
         * Update all the passwords of uid if required
         *
         * @param string uid
         * @param string password
         */
        void updatePasswords(string uid, string password);
    }
}