using CE_API_V2.Models;

namespace CE_API_V2.Utility
{
    public static class UserModelUpdater 
    {
        public static UserModel UpdateUserModel(UserModel updatedUser, UserModel storedUser, out List<string> errors)
        {
            errors = new List<string>();

            storedUser.Salutation = UpdatePropertyIfChanged(updatedUser.Salutation, storedUser.Salutation);
            storedUser.Title = UpdatePropertyIfChanged(updatedUser.Title, storedUser.Title);
            storedUser.Surname = UpdatePropertyIfChanged(updatedUser.Surname, storedUser.Surname);
            storedUser.FirstName = UpdatePropertyIfChanged(updatedUser.FirstName, storedUser.FirstName);
            storedUser.ProfessionalSpecialisation = UpdatePropertyIfChanged(updatedUser.ProfessionalSpecialisation, storedUser.ProfessionalSpecialisation);
            storedUser.PreferredLab = UpdatePropertyIfChanged(updatedUser.PreferredLab, storedUser.PreferredLab);
            storedUser.Address = UpdatePropertyIfChanged(updatedUser.Address, storedUser.Address);
            storedUser.City = UpdatePropertyIfChanged(updatedUser.City, storedUser.City);
            storedUser.ZipCode = UpdatePropertyIfChanged(updatedUser.ZipCode, storedUser.ZipCode);
            storedUser.CountryCode = UpdatePropertyIfChanged(updatedUser.CountryCode, storedUser.CountryCode);
            storedUser.Country = UpdatePropertyIfChanged(updatedUser.Country, storedUser.Country);
            storedUser.TelephoneNumber = UpdatePropertyIfChanged(updatedUser.TelephoneNumber, storedUser.TelephoneNumber);
            storedUser.EMailAddress = UpdatePropertyIfChanged(updatedUser.EMailAddress, storedUser.EMailAddress);
            storedUser.Language = UpdatePropertyIfChanged(updatedUser.Language, storedUser.Language);
            storedUser.UnitLabValues = UpdatePropertyIfChanged(updatedUser.UnitLabValues, storedUser.UnitLabValues);
            storedUser.Billing = UpdatePropertyIfChanged(updatedUser.Billing, storedUser.Billing);

            return storedUser;
        }

        public static UserModel UpdatePrivilegedData(UserModel updatedUser, UserModel storedUser, out List<string> errors)
        {
            errors = new List<string>();

            storedUser.IsActive = UpdatePropertyIfChanged(updatedUser.IsActive, storedUser.IsActive);
            storedUser.ClinicalSetting = UpdatePropertyIfChanged(updatedUser.ClinicalSetting, storedUser.ClinicalSetting);

            return storedUser;
        }
        private static T UpdatePropertyIfChanged<T>(T sourceValue, T destinationValue) => sourceValue is not null && !sourceValue.Equals(destinationValue) ? sourceValue : destinationValue;

    }
}
