namespace CE_API_V2.Services.Interfaces;

/// <summary>
/// Helper class to validate email related properties.
/// </summary>
public interface IEmailValidator
{
    /// <summary>
    /// Checks if the given string can be converted to a valid email address.
    /// </summary>
    /// <param name="eMail">String containing the email address which is validated.</param>
    /// <returns>Value indicating if a given string is a valid email address.</returns>
    public bool EmailAddressIsValid(string? eMail);
}