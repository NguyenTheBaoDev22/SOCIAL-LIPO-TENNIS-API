using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure; // 👈 cần thiết cho DatabaseFacade

namespace Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task EnsurePostgresFunctionsAsync(this AppDbContext context)
        {
            // 1️⃣ Kiểm tra và tạo `ufn_generate_merchant_code`
            const string checkMerchantCodeFn = @"
                SELECT COUNT(*) 
                FROM pg_proc 
                WHERE proname = 'ufn_generate_merchant_code';";

            var merchantCodeFnExists = await context.Database
                .ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM pg_proc WHERE proname = 'ufn_generate_merchant_code';");

            if (merchantCodeFnExists == 0)
            {
                const string createMerchantCodeFn = @"
CREATE OR REPLACE FUNCTION ufn_generate_merchant_code(id INT)
RETURNS TEXT AS $$
DECLARE
    chars TEXT := '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    result TEXT := '';
    num INT := id - 1;
BEGIN
    IF num < 0 THEN RETURN NULL; END IF;
    IF num = 0 THEN RETURN '000'; END IF;
    WHILE num > 0 LOOP
        result := substring(chars from (num % 36) + 1 for 1) || result;
        num := num / 36;
    END LOOP;
    RETURN lpad(result, 3, '0');
END;
$$ LANGUAGE plpgsql IMMUTABLE;
";
                await context.Database.ExecuteSqlRawAsync(createMerchantCodeFn);
            }

            // 2️⃣ Kiểm tra và tạo `ufn_generate_merchant_branch_code`
            const string checkBranchCodeFn = @"
                SELECT COUNT(*) 
                FROM pg_proc 
                WHERE proname = 'ufn_generate_merchant_branch_code';";

            var branchCodeFnExists = await context.Database
                .ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM pg_proc WHERE proname = 'ufn_generate_merchant_branch_code';");

            if (branchCodeFnExists == 0)
            {
                const string createBranchCodeFn = @"
CREATE OR REPLACE FUNCTION ufn_generate_merchant_branch_code(merchant_id UUID, sequence INT)
RETURNS TEXT AS $$
DECLARE
    chars TEXT := '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    result TEXT := '';
    num INT := sequence;
BEGIN
    IF num < 1 THEN RETURN '001'; END IF;
    WHILE num > 0 LOOP
        result := substring(chars from (num % 36) + 1 for 1) || result;
        num := num / 36;
    END LOOP;
    RETURN lpad(result, 3, '0');
END;
$$ LANGUAGE plpgsql IMMUTABLE;
";
                await context.Database.ExecuteSqlRawAsync(createBranchCodeFn);
            }
        }

        // Helper dùng để thực thi scalar query đơn giản
        private static async Task<T> ExecuteScalarAsync<T>(this DatabaseFacade database, string sql)
        {
            using var command = database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            if (command.Connection.State != System.Data.ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T))!;
        }
    }
}
