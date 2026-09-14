using GameOrderManager.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

//Program.cs作用為「先註冊 MVC 和 DbContext 等服務，Build 之後再設定 HTTPS、Routing 等請求流程，
//最後用 MapControllerRoute 設定 MVC 路由。」

// 加入 MVC 功能，讓專案可以使用 Controller 與 View。
builder.Services.AddControllersWithViews();

// 將 GameOrderContext 註冊到 ASP.NET Core 的 DI 容器。
// 並指定使用 SQL Server，以及 appsettings.json 裡的連線字串。
builder.Services.AddDbContext<GameOrderContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GameOrderContext")));
// 完成服務設定後，建立 Web Application。
var app = builder.Build();


// 非開發環境發生例外時，導向錯誤處理頁面。
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // 要求瀏覽器之後優先使用 HTTPS 連線。
    app.UseHsts();
}
// 將 HTTP 請求重新導向 HTTPS。
app.UseHttpsRedirection();
// 啟用 Routing（路由），
// 讓 ASP.NET Core 判斷網址應該交給哪個 Controller / Action。
app.UseRouting();
// 啟用授權功能。
// 目前專案沒有登入系統，但先保留範本設定。
app.UseAuthorization();
// 提供 CSS、JavaScript、圖片等靜態檔案。
app.MapStaticAssets();
// 設定 MVC 的預設路由。
// 例如 /Orders/Edit/5
// controller = Orders
// action = Edit
// id = 5
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Orders}/{action=Index}/{id?}")
    .WithStaticAssets();

// 啟動網站並開始接收 Request（請求）。
app.Run();
