namespace OCP.AppFramework.Http
{
/**
 * Redirects to the default app
 * @since 16.0.0
 */
    class RedirectToDefaultAppResponse : RedirectResponse {


    /**
     * Creates a response that redirects to the default app
     * @since 16.0.0
     */
    public function __construct() {
        parent::__construct(\OC_Util::getDefaultPageUrl());
    }

    }
}