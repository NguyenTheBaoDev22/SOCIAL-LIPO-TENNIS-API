using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random _random = new();

        public static string GenerateNumericCode(int length)
        {
            return string.Concat(Enumerable.Range(0, length).Select(_ => _random.Next(0, 10)));
        }
    }
}
