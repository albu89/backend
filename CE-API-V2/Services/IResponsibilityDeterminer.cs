namespace CE_API_V2.Services;

/// <summary>
/// Class to determine the email address which is used to inform a person in charge or point of contact for the user creation / onboarding process.
/// </summary>
public interface IResponsibilityDeterminer
{
    /// <summary>
    /// Get the email address of the person in charge or the responsible point of contact.
    /// </summary>
    /// <param name="country">The country where the user is professionally located.</param>
    /// <param name="organizationName">The name of the organization</param>
    /// <param name="wasCreatedExternally">Value indicating whether the user has been created externally.</param>
    /// <returns></returns>
    public string GetEmailAddress(string country, string organizationName, bool wasCreatedExternally);
}