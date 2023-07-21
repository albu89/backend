using System.Globalization;
using System.Runtime.Serialization;
using CE_API_V2.Constants;
using FluentValidation;
using FluentValidation.Results;
namespace CE_API_V2.Models.Exceptions;

public class BiomarkersValidationException : ValidationException
{
    public CultureInfo UserCulture { get; set; } = new CultureInfo(LocalizationConstants.DefaultLocale);

    public BiomarkersValidationException(string message) : base(message)
    {
    }
    public BiomarkersValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message, errors)
    {
    }
    public BiomarkersValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage) : base(message, errors, appendDefaultMessage)
    {
    }
    public BiomarkersValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
    {
    }
    public BiomarkersValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    
    public BiomarkersValidationException(string message, IEnumerable<ValidationFailure> errors, CultureInfo userCulture) : base(message, errors)
    {
        UserCulture = userCulture;
    }
}