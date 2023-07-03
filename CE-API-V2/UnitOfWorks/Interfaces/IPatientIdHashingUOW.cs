namespace CE_API_V2.Hasher
{
public interface IPatientIdHashingUOW
{
        public string HashPatientId(string firstName, string lastName, DateTimeOffset dateOfBirth);

        public bool VerifyPatientId(string firstName, string lastName, DateTimeOffset dateOfBirth, string hashedPatientInfo);
    }
}