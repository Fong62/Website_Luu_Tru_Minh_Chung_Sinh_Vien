using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HoatDongSinhVien.Pages.Shared
{
    public static class GenerateCode
    {
        public static string TaoMaMoi<TEntity>(
            DbContext context,
            DbSet<TEntity> dbSet,
            string prefix,
            string idFieldName) where TEntity : class
        {
            using var transaction = context.Database.BeginTransaction();

            var lastEntity = dbSet
                .OrderByDescending(e => EF.Property<object>(e, idFieldName))
                .FirstOrDefault();

            int lastNumber = 0;

            if (lastEntity != null)
            {
                var prop = typeof(TEntity).GetProperty(idFieldName, BindingFlags.Public | BindingFlags.Instance);
                var value = prop?.GetValue(lastEntity)?.ToString();

                if (!string.IsNullOrEmpty(value) && value.StartsWith(prefix))
                {
                    int.TryParse(value[prefix.Length..], out lastNumber);
                }
            }

            var newCode = $"{prefix}{(++lastNumber):D4}";
            transaction.Commit();
            return newCode;
        }
        public static string TaoMaMoi2(string prefix)
        {
            var code = $"{prefix}{Guid.NewGuid().ToString("N")[..4].ToUpper()}";
            return code;
        }
    }
}
