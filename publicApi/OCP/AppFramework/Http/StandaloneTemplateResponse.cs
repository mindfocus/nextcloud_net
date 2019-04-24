namespace OCP.AppFramework.Http
{
/**
 * A template response that does not emit the loadAdditionalScripts events.
 *
 * This is useful for pages that are authenticated but do not yet show the
 * full nextcloud UI. Like the 2FA page, or the grant page in the login flow.
 *
 * @since 16.0.0
 */
    class StandaloneTemplateResponse : TemplateResponse {

    }
}