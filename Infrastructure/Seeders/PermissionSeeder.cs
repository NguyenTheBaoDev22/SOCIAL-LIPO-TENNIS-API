using Core.Entities.AppUsers;
using Core.Enumerables;
using Humanizer;

namespace Infrastructure.Seeders
{
    public static class PermissionSeeder
    {
        //public static List<Permission> GetDefaultPermissions()
        //{
        //    return PermissionConstants.Map.Select(pair => new Permission
        //    {
        //        Id = pair.Value,
        //        Code = pair.Key, // <-- FIXED: không cần ToString()
        //        Description = pair.Key.ToString().Humanize(LetterCasing.Sentence)
        //    }).ToList();
        //}
    }
}
