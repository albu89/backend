namespace CE_API_V2.Services.Interfaces
{
    /// <summary>
    /// Helper class providing templates for the email body.
    /// </summary>
    public interface IEmailTemplateProvider
    {
        /// <summary>
        /// Get the request access body email template.
        /// </summary>
        /// <returns>String containing the body of the request access email.</returns>
        public string GetRequestBodyTemplate();

        /// <summary>
        /// Get request body email template.
        /// </summary>
        /// <returns>String containing the body of the activate user email.</returns>
        public string GetActivateUserBodyTemplate();
    }
}