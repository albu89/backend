using System.Net.Mail;
using CE_API_V2.Services.Interfaces;

namespace CE_API_V2.Services;

/// <inheritdoc/>
public class EmailValidator : IEmailValidator
{
    /// <inheritdoc/>
    public bool EmailAddressIsValid(string? eMail) => eMail is not null && MailAddress.TryCreate(eMail, out _);
}