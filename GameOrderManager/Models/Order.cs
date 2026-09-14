using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace GameOrderManager.Models
{ 
    // 定義一筆訂單的資料結構。
    // EF Core 會依照這個 Model 對應 Orders 資料表。
    public class Order
    {
        // 訂單主鍵。
        // EF Core 會將名稱為 Id 的欄位預設視為 Primary Key（主鍵）。
        public int Id { get; set; }

        //Data Annotations（資料註解），主要拿來描述驗證規則。
        // 客戶名稱：必填，最多 100 個字元。
        [Required(ErrorMessage = "顧客名を入力してください。")]
        [StringLength(100, ErrorMessage = "顧客名は100文字以内で入力してください。")]
        public string CustomerName { get; set; } = "";

        // 商品名稱：必填，最多 100 個字元。
        [Required(ErrorMessage = "商品名を入力してください。")]
        [StringLength(100, ErrorMessage = "商品名は100文字以内で入力してください。")]
        public string ProductName { get; set; } = "";

        // 訂購數量：限定為 1～999。
        [Required(ErrorMessage = "数量を入力してください。")]
        [Range(1, 999, ErrorMessage = "数量は1～999の範囲で入力してください。")]
        public int Quantity { get; set; }

        // 單價：
        // Precision(20, 2) 指定 SQL Server decimal 欄位最多 20 位，
        // 其中小數點後保留 2 位。
        [Required(ErrorMessage = "単価を入力してください。")]
        [Precision(20,2)]
        [Range(typeof(decimal), "0.01", "9999999",
            ErrorMessage = "単価は0.01～9,999,999の範囲で入力してください。")]
        public decimal UnitPrice { get; set; }

        // 訂單處理狀態：必填，最多 20 個字元。
        // 新建立的 Order 預設為「未處理」。
        [Required(ErrorMessage = "ステータスを選択してください。")]
        [StringLength(20, ErrorMessage = "ステータスは20文字以内で入力してください。")]
        public string Status { get; set; } = "未処理";

        // 遊戲平台：必填，最多 30 個字元。
        [Required(ErrorMessage = "プラットフォームを選択してください。")]
        [StringLength(30, ErrorMessage = "プラットフォームは30文字以内で入力してください。")]
        public string Platform { get; set; } = "";
    }
}
