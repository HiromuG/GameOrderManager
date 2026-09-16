# GameOrderManager

ASP.NET Core MVC と SQL Server を使って作成した、ゲーム商品の受注管理 Web アプリケーションです。

受注データの登録・編集・削除などの CRUD 操作に加えて、
検索、ステータスによる絞り込み、並び替えなどの機能を実装しています。

## 画面イメージ

### 受注一覧

![受注一覧](screenshots/order-list.png)

### 新規受注

![新規受注](screenshots/create-order.png)

### 受注詳細

![受注詳細](screenshots/order-details.png)

## 主な機能

- 受注一覧表示
- 新規受注登録
- 受注内容の編集
- 受注詳細表示
- 受注削除
- 顧客名・商品名・プラットフォームによるキーワード検索
- ステータスによる絞り込み
- 受注番号・数量・単価による並び替え
- ステータス別の受注件数表示
- 入力値のバリデーション

## 使用技術

- C#
- .NET 10
- ASP.NET Core MVC
- Entity Framework Core 10
- SQL Server LocalDB
- Razor
- Bootstrap
- Git / GitHub

## データベース

Entity Framework Core の Migration を使用し、
Model の変更を SQL Server のデータベースに反映しています。

受注データとして、以下の項目を管理しています。

- 顧客名
- 商品名
- 数量
- 単価
- プラットフォーム
- ステータス

## 開発目的

以前に触れた ASP.NET MVC と SQL Server の復習を兼ねて、
ASP.NET Core MVC と Entity Framework Core を使って作成しました。

CRUD の基本を確認するだけではなく、
検索条件の組み合わせや並び替え、Migration を使ったデータベース変更なども実際に実装し、
Web アプリケーションの基本的な流れを学び直すことを目的としています。

## 実行方法

1. このリポジトリをクローンします。
2. Visual Studio でソリューションを開きます。
3. SQL Server LocalDB が利用できる環境で、Package Manager Console から以下を実行します。

```powershell
Update-Database
