using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enumerables
{
    public static class PaymentMethodConst
    {
        public const string Cash = "CASH";
        public const string BankTransfer = "BANK";
        public const string Card = "CARD";

        public static readonly Dictionary<string, string> DisplayNames = new()
    {
        { Cash, "Tiền mặt" },
        { BankTransfer, "Chuyển khoản" },
        { Card, "Quẹt thẻ" }
    };
    }
}
