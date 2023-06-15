using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
namespace CE_API_V2.UnitOfWorks;

public class PatientIdHashingUOW : IPatientIdHashingUOW
{

    public string GeneratePatientId(string name, string lastName, DateTimeOffset dateOfBirth)
    {
        // NOT a hash!!! Gets Implemented in CE-34-Store-hashed-PatientId
        return name + lastName + dateOfBirth.Date.ToLongDateString();
    }
}