using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CE_API_V2.Hasher
{
    public class PatientIdHashingUOW : IPatientIdHashingUOW
    {
        private readonly IConfiguration _config;

        public PatientIdHashingUOW(IConfiguration config)
        {
            _config = config;
        }

        public string HashPatientId(string firstName, string lastName, DateTime dateOfBirth)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(firstName + lastName + dateOfBirth.Date.Ticks);
                // Immediately dereference the values once used
                firstName = null;
                lastName = null;
                dateOfBirth = new DateTime();
        
                if (_config["Salt"].IsNullOrEmpty())
                {
                    throw new NotImplementedException();
                }

                using HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_config["Salt"]));
                byte[] hashValue = hmac.ComputeHash(bytes);
                string hash = Convert.ToBase64String(hashValue);

                return hash;
            }
            catch (Exception)
            {
                throw new NotImplementedException("Patient Id could not be hashed.");
            }
        }

        public bool VerifyPatientId(string firstName, string lastName, DateTime dateOfBirth, string hashedPatientInfo)
        {
            try
            {
                byte[] patientInfoBytes = Encoding.UTF8.GetBytes(firstName + lastName + dateOfBirth.Date.Ticks);
                // Immediately dereference the values once used
                firstName = null;
                lastName = null;
                dateOfBirth = new DateTime();
                bool err = false;

                using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_config["Salt"])))
                {
                    byte[] hashedPatientInfoBytes = Encoding.UTF8.GetBytes(hashedPatientInfo);

                    byte[] computedHash = hmac.ComputeHash(patientInfoBytes);

                    for (int i = 0; i < hashedPatientInfoBytes.Length; i++)
                    {
                        if (computedHash[i] != hashedPatientInfoBytes[i])
                        {
                            err = true;
                        }
                    }
                }

                return !err;
            }
            catch (Exception)
            {
                throw new NotImplementedException("Patiend Id could not be validated");
            }
        }
    }
}