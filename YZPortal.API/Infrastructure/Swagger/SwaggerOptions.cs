namespace YZPortal.API.Infrastructure.Swagger
{
    public class SwaggerOptions
    {
        // REF: https://stackoverflow.com/questions/52639632/is-there-support-for-multiple-oauth2-authentication-login-providers-in-swaggerui,
        // https://github.com/swagger-api/swagger-ui/issues/4690, 
        // Multiple OAuth2 providers not supported atm, default OAuth2 provider is Azure AD B2C, set true to enable Azure AD.
        // Can remove this options once we figure out how to impletement multiple OAuth2 provider for swagger ui
        public bool IsAzureAdOAuth2Provider { get; set; }
    }
}
