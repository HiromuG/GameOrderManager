using GameOrderManager.Data;
using GameOrderManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GameOrderManager.Controllers
{
    public class OrdersController : Controller
    {
        private readonly GameOrderContext _context;

        public OrdersController(GameOrderContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? keyword, string? status, string? sort)
        { 
            // 建立 Orders 的查詢物件。
            // 此時還沒有真正把資料全部從 SQL Server 讀取出來，
            var orders = _context.Orders.AsQueryable();

            // 關鍵字搜尋：
            // keyword 有值時，搜尋客戶名稱、商品名稱或平台。
            if (!string.IsNullOrEmpty(keyword))
            {
                orders = orders.Where(o =>
                    o.CustomerName.Contains(keyword) ||
                    o.ProductName.Contains(keyword) ||
                    o.Platform.Contains(keyword)
                );
            }

            // 狀態篩選：
            // status 有值時，只保留狀態完全相同的訂單。
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => 
                    o.Status == status);
            }

            // Dashboard 顯示用的訂單統計。
            // 這裡統計的是資料庫中的全部訂單，不受上面的搜尋條件影響。
            ViewData["TotalOrders"] = _context.Orders.Count();
            ViewData["PendingOrders"] = _context.Orders.Count(o => o.Status == "未處理");
            ViewData["ProcessingOrders"] = _context.Orders.Count(o => o.Status == "處理中");
            ViewData["CompletedOrders"] = _context.Orders.Count(o => o.Status == "已完成");

            // 將目前使用中的搜尋、篩選及排序條件傳給 View，
            // 讓頁面重新載入後仍能保留使用者選擇的內容。
            ViewData["CurrentKeyword"] = keyword;
            ViewData["CurrentStatus"] = status;
            ViewData["CurrentSort"] = sort;

            //依價格排序
            if (sort == "price_asc")
            {
                orders = orders.OrderBy(o => o.UnitPrice);
            }
            else if (sort == "price_desc")
            {
                orders = orders.OrderByDescending(o => o.UnitPrice);
            }
            //依編號排序
            else if (sort == "id_asc")
            {
                orders = orders.OrderBy(o => o.Id);
            }
            else if (sort == "id_desc")
            {
                orders = orders.OrderByDescending(o => o.Id);
            }
            //依數量排序
            else if (sort == "quantity_asc")
            {
                orders = orders.OrderBy(o => o.Quantity);
            }
            else if (sort == "quantity_desc")
            {
                orders = orders.OrderByDescending(o => o.Quantity);
            }
            // ToList() 執行查詢，實際從資料庫取得符合條件的資料，
            // 再將 List<Order> 傳給 Index View。
            return View(orders.ToList());
        }

        // 顯示新增訂單頁面。
        // GET /Orders/Create
        public IActionResult Create()
        {
            return View();
        }
        // 接收新增訂單表單送出的資料。
        // POST /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            // 檢查 Order 是否符合 Model 上設定的驗證規則。
            if (ModelState.IsValid)
            {
                // 將新訂單加入 EF Core 的追蹤中。
                _context.Orders.Add(order);
                // 將新增內容實際寫入 SQL Server。
                _context.SaveChanges();
                // 新增成功後回到訂單列表。
                return RedirectToAction(nameof(Index));
            }
            // 驗證失敗時，不寫入資料庫，
            // 並將使用者原本輸入的內容重新顯示在表單中。
            return View(order);
        }

        // 顯示修改訂單頁面。
        // GET /Orders/Edit/5
        public IActionResult Edit(int id)
        {
            // 依照主鍵 Id 從資料庫尋找指定訂單。
            var order = _context.Orders.Find(id);
            // 如果找不到指定 Id 的資料，回傳 HTTP 404。
            if (order == null)
            {
                return NotFound();
            }
            // 將找到的訂單傳給 Edit View，
            // 讓表單顯示原本的資料。
            return View(order);
        }
        // 接收修改後的訂單資料。
        // POST /Orders/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            // 檢查修改後的 Order 是否符合 Model 驗證規則。
            if (ModelState.IsValid)
            {
                // 告訴 EF Core 這筆 Order 已被修改。
                _context.Orders.Update(order);
                // 將修改內容實際寫入 SQL Server。
                _context.SaveChanges();
                // 修改成功後回到訂單列表。
                return RedirectToAction(nameof(Index));
            }
            // 驗證失敗時，不更新資料庫，
            // 並重新顯示使用者修改過的內容。
            return View(order);
        }

        // 顯示刪除確認頁面。
        // GET /Orders/Delete/5
        public IActionResult Delete(int id)
        {
            // 依照主鍵 Id 尋找要刪除的訂單。
            var order = _context.Orders.Find(id);

            // 找不到指定資料時回傳 HTTP 404。
            if (order == null)
            {
                return NotFound();
            }

            // 將訂單資料傳給 Delete View，
            // 讓使用者確認是否真的要刪除。
            return View(order);
        }
        // 接收刪除確認表單。
        [HttpPost]
        //後面都是int id 的話Action不能同名所以加上這行給asp-action呼叫用
        //所以對 MVC 而言 Action 名稱仍是 Delete。
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // 再次依照 Id 從資料庫尋找訂單。
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            // 告訴 EF Core 要刪除這筆訂單。
            _context.Orders.Remove(order);
            // 將刪除動作實際寫入 SQL Server。
            _context.SaveChanges();
            // 刪除完成後回到訂單列表。
            return RedirectToAction(nameof(Index));
        }

        // 顯示指定訂單的詳細資料。
        // GET /Orders/Details/5
        public IActionResult Details(int id)
        {
            // 依照主鍵 Id 從資料庫尋找訂單。
            var order = _context.Orders.Find(id);

            // 找不到指定訂單時回傳 HTTP 404。
            if (order == null)
            {
                return NotFound();
            }
            // 將找到的單筆 Order 傳給 Details View。
            return View(order);
        }
    }
}