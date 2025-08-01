using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enumerables
{
    public static class BusinessTypeConstants
    {
        public static readonly List<BusinessTypeInfo> BusinessTypes = new()
        {
            new BusinessTypeInfo("Household", "Cá nhân/Hộ kinh doanh"),
            new BusinessTypeInfo("Company", "Doanh nghiệp")
        };

        public static readonly string[] AllCodes = BusinessTypes.Select(x => x.Code).ToArray();
    }

    public record BusinessTypeInfo(string Code, string Name);
}
