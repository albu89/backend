namespace CE_API_V2.UnitOfWorks.Interfaces;

public interface IPatientIdHashingUOW
{
    string GeneratePatientId(string name, string lastName, DateTimeOffset dateOfBirth);
}