# POYAN | پویان

سامانه یکپارچه پایش و ارزیابی رشد آموزشی و ورزشی دانش‌آموزان.

## فناوری‌ها
- ASP.NET Core Razor Pages و C# روی .NET 10
- Entity Framework Core 10 و SQL Server
- ASP.NET Core Identity و نقش‌های پایه
- رابط فارسی و راست‌چین

## ساختار فعلی
```text
POYAN/
├── POYAN.Web/
│   ├── Data/          # EF Core DbContext
│   ├── Models/        # School, Student, GrowthRecord, ApplicationUser
│   ├── Pages/         # Razor Pages
│   ├── Security/      # Claims و محدوده مدرسه
│   └── wwwroot/       # فایل‌های استاتیک
├── .github/workflows/ # CI
└── PROGRESS.md
```

## اجرای محلی
پیش‌نیازها: .NET 10 SDK و SQL Server یا SQL Server LocalDB.

1. مقدار `ConnectionStrings:DefaultConnection` را در User Secrets یا متغیر محیطی `ConnectionStrings__DefaultConnection` تنظیم کنید.
2. Migration اولیه را بسازید و دیتابیس را به‌روزرسانی کنید:

```bash
dotnet ef migrations add InitialCreate --project POYAN.Web --startup-project POYAN.Web
dotnet ef database update --project POYAN.Web --startup-project POYAN.Web
```
3. اجرا:

```bash
dotnet restore POYAN.Web/POYAN.Web.csproj
dotnet run --project POYAN.Web/POYAN.Web.csproj
```

> فعلاً Migration اولیه و گردش ایجاد اولین Super Admin در دست توسعه است؛ برنامه هنوز برای استقرار عملیاتی آماده نیست.

## تنظیم اتصال AI
سرویس AI از APIهای سازگار با OpenAI Chat Completions پشتیبانی می‌کند. تنظیمات غیرمحرمانه را در بخش `Ai` قرار دهید و کلید را فقط از Secret Manager یا متغیر محیطی `Ai__ApiKey` وارد کنید. تا زمانی که Endpoint، Model و کلید معتبر تنظیم نشوند، سرویس درخواست ارسال نمی‌کند.

## امنیت و چندمدرسه‌ای
- کاربران عادی باید Claim معتبر `school_id` داشته باشند.
- Query Filter برای مدرسه، دانش‌آموز و رکورد رشد اعمال شده است.
- عملیات نوشتن باید شناسه مدرسه را از هویت احراز‌شده سمت سرور دریافت کند؛ هرگز به SchoolId ارسالی از مرورگر اعتماد نکنید.
- کلیدهای سرویس AI و رمزهای اتصال نباید در مخزن Git قرار بگیرند.

## وضعیت پروژه
جزئیات مراحل، موارد تکمیل‌شده و کارهای باقی‌مانده در [PROGRESS.md](PROGRESS.md) ثبت می‌شود.
