using Shared.Authorization.Shared.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Authorization
{
    public static class RoleConstants
    {
        public static readonly Dictionary<RoleCode, Guid> Map = new()
    {
        { RoleCode.Owner, Guid.Parse("f5f1dc90-7a3e-4f01-9f12-95d123456001") },
        { RoleCode.Manager, Guid.Parse("f5f1dc90-7a3e-4f01-9f12-95d123456002") },
        { RoleCode.Cashier, Guid.Parse("f5f1dc90-7a3e-4f01-9f12-95d123456003") },
        { RoleCode.InventoryStaff, Guid.Parse("f5f1dc90-7a3e-4f01-9f12-95d123456004") },
        { RoleCode.Accountant, Guid.Parse("f5f1dc90-7a3e-4f01-9f12-95d123456005") },
        { RoleCode.CustomerSupport, Guid.Parse("f5f1dc90-7a3e-4f01-9f12-95d123456006") },
        { RoleCode.Admin, Guid.Parse("f5f1dc90-7a3e-4f01-9f12-95d123456007") }
    };

        public static void ExportToJson(string filePath)
        {
            var exportData = Map.ToDictionary(
                x => x.Key.ToString(),
                x => new { Id = x.Value, Name = GetDisplayName(x.Key) }
            );

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };

            File.WriteAllText(filePath, JsonSerializer.Serialize(exportData, options));
        }

        public static string GetDisplayName(RoleCode code)
        {
            var display = code.GetType()
                .GetField(code.ToString())?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

            return display?.Name ?? code.ToString();
        }
    }
}
