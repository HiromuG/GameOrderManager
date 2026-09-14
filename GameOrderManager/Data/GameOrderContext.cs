using GameOrderManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOrderManager.Data
{
    // 負責 EF Core 與資料庫之間的連線與資料操作。
    public class GameOrderContext : DbContext
    {
        // DbContextOptions 會由 ASP.NET Core 的 DI（依賴注入）
        // 傳入資料庫連線等設定。
        public GameOrderContext(DbContextOptions<GameOrderContext> options)
            : base(options)
        {
        }

        // 代表資料庫中的 Orders 資料集合。
        // Order 是單筆訂單的 Model，
        // Orders 則可以用來查詢、新增、修改及刪除訂單資料。
        public DbSet<Order> Orders { get; set; }
    }
}