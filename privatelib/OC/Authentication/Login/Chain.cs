namespace OC.Authentication.Login
{
    public class Chain
    {
        /** @var PreLoginHookCommand */
	private PreLoginHookCommand preLoginHookCommand;

	/** @var UserDisabledCheckCommand */
	private UserDisabledCheckCommand userDisabledCheckCommand;

	/** @var UidLoginCommand */
	private UidLoginCommand uidLoginCommand;

	/** @var EmailLoginCommand */
	private EmailLoginCommand emailLoginCommand;

	/** @var LoggedInCheckCommand */
	private LoggedInCheckCommand loggedInCheckCommand;

	/** @var CompleteLoginCommand */
	private CompleteLoginCommand completeLoginCommand;

	/** @var CreateSessionTokenCommand */
	private CreateSessionTokenCommand createSessionTokenCommand;

	/** @var ClearLostPasswordTokensCommand */
	private ClearLostPasswordTokensCommand clearLostPasswordTokensCommand;

	/** @var UpdateLastPasswordConfirmCommand */
	private UpdateLastPasswordConfirmCommand updateLastPasswordConfirmCommand;

	/** @var SetUserTimezoneCommand */
	private SetUserTimezoneCommand setUserTimezoneCommand;

	/** @var TwoFactorCommand */
	private TwoFactorCommand twoFactorCommand;

	/** @var FinishRememberedLoginCommand */
	private FinishRememberedLoginCommand finishRememberedLoginCommand;

	public Chain (PreLoginHookCommand preLoginHookCommand,
								UserDisabledCheckCommand userDisabledCheckCommand,
								UidLoginCommand uidLoginCommand,
								EmailLoginCommand emailLoginCommand,
								LoggedInCheckCommand loggedInCheckCommand,
								CompleteLoginCommand completeLoginCommand,
								CreateSessionTokenCommand createSessionTokenCommand,
								ClearLostPasswordTokensCommand clearLostPasswordTokensCommand,
								UpdateLastPasswordConfirmCommand updateLastPasswordConfirmCommand,
								SetUserTimezoneCommand setUserTimezoneCommand,
								TwoFactorCommand twoFactorCommand,
								FinishRememberedLoginCommand finishRememberedLoginCommand
	) {
		this.preLoginHookCommand = preLoginHookCommand;
		this.userDisabledCheckCommand = userDisabledCheckCommand;
		this.uidLoginCommand = uidLoginCommand;
		this.emailLoginCommand = emailLoginCommand;
		this.loggedInCheckCommand = loggedInCheckCommand;
		this.completeLoginCommand = completeLoginCommand;
		this.createSessionTokenCommand = createSessionTokenCommand;
		this.clearLostPasswordTokensCommand = clearLostPasswordTokensCommand;
		this.updateLastPasswordConfirmCommand = updateLastPasswordConfirmCommand;
		this.setUserTimezoneCommand = setUserTimezoneCommand;
		this.twoFactorCommand = twoFactorCommand;
		this.finishRememberedLoginCommand = finishRememberedLoginCommand;
	}

	public LoginResult process(LoginData loginData) {
		var chain = this.preLoginHookCommand;
		chain
			.setNext(this.userDisabledCheckCommand)
			.setNext(this.uidLoginCommand)
			.setNext(this.emailLoginCommand)
			.setNext(this.loggedInCheckCommand)
			.setNext(this.completeLoginCommand)
			.setNext(this.createSessionTokenCommand)
			.setNext(this.clearLostPasswordTokensCommand)
			.setNext(this.updateLastPasswordConfirmCommand)
			.setNext(this.setUserTimezoneCommand)
			.setNext(this.twoFactorCommand)
			.setNext(this.finishRememberedLoginCommand);

		return chain.process(loginData);
		}
    }
}