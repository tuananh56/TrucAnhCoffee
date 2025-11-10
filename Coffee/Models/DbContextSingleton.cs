using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public sealed class DbContextSingleton
    {
        // Tạo một thể hiện duy nhất bằng Lazy<T> (Thread-safe)
        private static readonly Lazy<DbContextSingleton> instance =
            new Lazy<DbContextSingleton>(() => new DbContextSingleton());

        public static DbContextSingleton Instance => instance.Value;

        public CafeDBEntities DbContext { get; private set; }

        // Constructor private để ngăn chặn khởi tạo từ bên ngoài
        private DbContextSingleton()
        {
            DbContext = new CafeDBEntities
            {
                Configuration =
                {
                    ProxyCreationEnabled = false,   // Tắt Proxy
                    LazyLoadingEnabled = false      // Tắt Lazy Loading
                }
            };
        }
    }

}