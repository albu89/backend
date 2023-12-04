using CE_API_V2.Data;
using CE_API_V2.Models;

namespace CE_API_Test.TestUtilities
{
    internal static class ContextSeeder
    {
        public static void InsertUser(CEContext ceContext, UserModel userModel)
        {
            if (ceContext.Users.Any())
            {
                return;
            }

            ceContext.Users.Add(userModel);
            ceContext.SaveChanges();
        }

        public static void UpdateUser(CEContext ceContext, UserModel userModel)
        {
            if (!ceContext.Users.Any())
            {
                return;
            }

            ceContext.Users.Update(userModel);
            ceContext.SaveChanges();
        }

        public static void RemoveUsers(CEContext ceContext, UserModel userModel)
        {
            if (!ceContext.Users.Any())
            {
                return;
            }

            var users = ceContext.Users;
            ceContext.Users.RemoveRange(users);

            ceContext.SaveChanges();
        }
    }
}
