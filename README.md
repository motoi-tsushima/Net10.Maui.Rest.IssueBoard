# 課題掲示板アプリケーション

## 概要

このソリューションは、.NET 10 を使用した課題掲示板システムです。
MAUIクライアント、ASP.NET Core Web API、共有DTOライブラリで構成されています。

## プロジェクト構成

1. **Api.Rest.IssueBoard** - ASP.NET Core Web API プロジェクト
   - RESTful API を提供
   - Entity Framework Core + SQL Server を使用
   - データベース名: `Samples`

2. **Shared.Rest.IssueBoard** - 共有ライブラリ
   - DTOクラス
   - Enumなど共通データ型

3. **Net10.Maui.Rest.IssueBoard** - .NET MAUI クライアント
   - Windows, Android, iOS対応
   - API経由でデータを操作

## 実装状況

### ? 完了している項目

#### API プロジェクト (Api.Rest.IssueBoard)
- [x] Models/Issue.cs - データモデル (scaffold済み)
- [x] Data/IssuesDbContext.cs - DbContext (scaffold済み)
- [x] Mapping/IssueMapper.cs - DTO⇔Model変換
- [x] Controllers/IssuesController.cs - REST API エンドポイント
- [x] Program.cs - サービス構成、CORS設定
- [x] appsettings.json - データベース接続文字列

#### 共有プロジェクト (Shared.Rest.IssueBoard)
- [x] IssueStatus.cs - ステータスEnum
- [x] IssueDto.cs - 課題DTO
- [x] CreateIssueDto.cs - 新規登録用DTO
- [x] UpdateIssueDto.cs - 更新用DTO

#### MAUI プロジェクト (Net10.Maui.Rest.IssueBoard)
- [x] Services/IssueService.cs - API通信サービス
- [x] Helpers/IssueStatusHelper.cs - ステータス表示ヘルパー
- [x] Helpers/PreferencesHelper.cs - 氏名保存ヘルパー
- [x] Views/*.xaml - すべての画面XAML
- [x] Views/*.xaml.cs - すべての画面コードビハインド
- [x] MauiProgram.cs - DI設定
- [x] AppShell.xaml/cs - ナビゲーション設定
- [x] Platforms/Windows/App.xaml.cs - ウィンドウサイズ・配置設定

### ?? 現在の問題

MAUIプロジェクトのビルドで以下のエラーが発生しています：
```
CS0103: 現在のコンテキストに 'InitializeComponent' という名前は存在しません
```

これはXAMLファイルのコード生成が正しく行われていないことを示しています。

## セットアップ手順

### 1. データベースの準備

SQL Serverが稼働していることを確認してください。
データベース `Samples` にテーブル `Issues` が既に作成されていることを前提としています。

もしテーブルがない場合は、以下のスクリプトで作成してください：

```sql
CREATE DATABASE Samples;
GO

USE Samples;
GO

CREATE TABLE Issues (
    Id INT PRIMARY KEY IDENTITY(1,1),
    AuthorName NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Category NVARCHAR(30),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(2000) NOT NULL,
    Status INT NOT NULL DEFAULT 0, -- 0:未着手, 1:着手中, 2:解決失敗, 3:課題確認不能, 4:解決済み
    Resolution NVARCHAR(2000),
    ResolverName NVARCHAR(50),
    ResolvedAt DATETIME
);
```

### 2. APIプロジェクトの実行

```powershell
cd Api.Rest.IssueBoard
dotnet run --launch-profile https
```

APIは `https://localhost:7001` で起動します。

### 3. MAUIプロジェクトのビルドエラー解決

現在、XAMLのコード生成に問題があります。以下の手順を試してください：

#### 方法1: Visual Studioでの修正
1. Visual Studioでソリューションを開く
2. `Net10.Maui.Rest.IssueBoard` プロジェクトを右クリック
3. 「Rebuild」を選択
4. それでもエラーが出る場合は、`obj`と`bin`フォルダーを手動で削除してから再ビルド

#### 方法2: コマンドラインでの修正
```powershell
# objとbinフォルダーを削除
Remove-Item -Path "Net10.Maui.Rest.IssueBoard\obj" -Recurse -Force
Remove-Item -Path "Net10.Maui.Rest.IssueBoard\bin" -Recurse -Force

# 復元とビルド
dotnet restore Net10.Maui.Rest.IssueBoard\Net10.Maui.Rest.IssueBoard.csproj
dotnet build Net10.Maui.Rest.IssueBoard\Net10.Maui.Rest.IssueBoard.csproj -f net10.0-windows10.0.19041.0
```

#### 方法3: XAMLファイルの再生成（最終手段）
もし上記で解決しない場合は、Visual Studioで：
1. `Views`フォルダーのXAMLファイルを一つずつ確認
2. 各XAMLファイルを開いて保存し直す
3. プロジェクトをリビルド

## API エンドポイント

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| GET | /api/issues | すべての課題を取得 |
| GET | /api/issues/{id} | 特定の課題を取得 |
| POST | /api/issues | 新規課題を登録 |
| PUT | /api/issues/{id} | 課題を更新 |
| DELETE | /api/issues/{id} | 課題を削除 |

## 画面一覧

1. **課題一覧 (IssueListPage)** - 登録された課題の一覧表示
2. **課題内容表示 (IssueDetailPage)** - 課題の詳細情報表示
3. **課題内容編集 (IssueEditPage)** - 課題の編集
4. **課題削除確認 (IssueDeletePage)** - 削除前の確認
5. **課題新規登録 (IssueCreatePage)** - 新規課題の登録

## 技術スタック

- **.NET 10**
- **MAUI** - クロスプラットフォームUI
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core** - ORM
- **SQL Server** - データベース

## 注意事項

- 認証機能はありません（学習用サンプルのため）
- 機密情報は登録しないでください
- 記入者氏名はローカルストレージに保存されます

## トラブルシューティング

### XAML コンパイルエラー
- `obj`と`bin`フォルダーを削除してリビルド
- Visual Studioを再起動
- .NET SDK 10 が正しくインストールされているか確認

### API接続エラー
- APIが`https://localhost:7001`で起動しているか確認
- CORS設定が正しいか確認
- 証明書の警告が出た場合は信頼する

### データベース接続エラー
- SQL Serverが起動しているか確認
- 接続文字列が正しいか確認（`appsettings.json`）
- データベース `Samples` が存在するか確認

## ライセンス

このプロジェクトは学習用サンプルです。
