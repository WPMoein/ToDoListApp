
````markdown
# 🔍 تحلیل کد پروژه TodoAppSQLite (Windows Forms + SQLite)

این فایل شامل تحلیل کامل فایل `MainForm.cs` از پروژه To-Do List نوشته‌شده با زبان C#، پلتفرم WinForms و پایگاه‌داده SQLite است.

---

## 📚 کتابخانه‌های استفاده‌شده

```csharp
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
````

| فضای نام (namespace)   | کاربرد                           |
| ---------------------- | -------------------------------- |
| `System`               | توابع پایه زبان سی‌شارپ          |
| `System.Data`          | ارتباطات دیتابیسی کلی            |
| `System.Data.SQLite`   | درایور مخصوص SQLite برای ADO.NET |
| `System.IO`            | بررسی و مدیریت فایل‌ها           |
| `System.Windows.Forms` | رابط گرافیکی ویندوز فرم‌ها       |

---

## 🧩 کلاس MainForm

```csharp
public partial class MainForm : Form
```

* `MainForm`: فرم اصلی رابط کاربری
* `partial`: نشان‌دهنده وجود تعریف مکمل در فایل Designer
* ارث‌بری از `Form`: نمایش فرم ویندوز

---

## 🛠 فیلدها و سازنده کلاس

```csharp
private const string DbFile = "tasks.db";
private SQLiteConnection connection;

public MainForm()
{
    InitializeComponent();
    InitDatabase();
    LoadTasks();
}
```

| بخش              | شرح                               |
| ---------------- | --------------------------------- |
| `DbFile`         | مسیر فایل دیتابیس SQLite          |
| `connection`     | اتصال باز به SQLite               |
| `InitDatabase()` | ایجاد دیتابیس و جدول در صورت نیاز |
| `LoadTasks()`    | بارگذاری تسک‌ها در رابط کاربری    |

---

## 🧱 InitDatabase - ساخت دیتابیس

```csharp
private void InitDatabase()
```

* بررسی وجود فایل `tasks.db`
* ساخت جدول `Tasks` با فیلدهای: `Id`, `Title`, `IsCompleted`, `DueDate`
* استفاده از `CREATE TABLE IF NOT EXISTS`

---

## 📥 LoadTasks - بارگذاری تسک‌ها

```csharp
private void LoadTasks()
```

* پاک کردن آیتم‌های فعلی لیست
* خواندن داده‌ها از جدول `Tasks`
* تبدیل به رشته نمایشی شامل وضعیت و تاریخ
* اضافه کردن به `taskList.Items`

---

## ➕ addButton\_Click - افزودن تسک جدید

```csharp
private void addButton_Click(object sender, EventArgs e)
```

* بررسی عدم خالی بودن ورودی
* گرفتن مقدار تاریخ از `DateTimePicker`
* ثبت تسک جدید با `IsCompleted = 0`
* استفاده از پارامتر برای جلوگیری از SQL Injection

---

## 🔄 toggleButton\_Click - تغییر وضعیت

```csharp
private void toggleButton_Click(object sender, EventArgs e)
```

* گرفتن ایندکس آیتم انتخابی در `taskList`
* یافتن شناسه (`Id`) و وضعیت فعلی از دیتابیس
* معکوس کردن وضعیت (`0` ↔ `1`)
* آپدیت در دیتابیس

---

## ❌ removeButton\_Click - حذف تسک

```csharp
private void removeButton_Click(object sender, EventArgs e)
```

* بررسی انتخاب آیتم
* استخراج `Id` از جدول با توجه به موقعیت
* اجرای دستور `DELETE FROM Tasks WHERE Id = @id`

---

## 🧰 کنترل‌های رابط گرافیکی (WinForms)

| کنترل UI                       | عملکرد                |
| ------------------------------ | --------------------- |
| `TextBox taskInput`            | ورودی عنوان تسک       |
| `DateTimePicker dueDatePicker` | انتخاب تاریخ انجام    |
| `ListBox taskList`             | نمایش لیست تسک‌ها     |
| `Button addButton`             | افزودن تسک            |
| `Button toggleButton`          | تغییر وضعیت انجام‌شده |
| `Button removeButton`          | حذف تسک               |

---

## 🔐 نکات امنیتی و فنی

* استفاده از **پارامتر در SQL** برای جلوگیری از حملات تزریق (`SQLiteCommand.Parameters`)
* استفاده از **reader.Close()** برای بستن منابع پس از خواندن
* ساختار ساده ولی مناسب برای یک پروژه CRUD محلی

---

## ✅ جمع‌بندی تکنیکی

| مورد         | نوع/تکنولوژی              | توضیح                              |
| ------------ | ------------------------- | ---------------------------------- |
| دیتابیس      | SQLite                    | ذخیره‌سازی سبک و لوکال             |
| لایه دسترسی  | ADO.NET (`SQLiteCommand`) | اجرای دستورات SQL                  |
| رابط گرافیکی | Windows Forms             | فرم سنتی در ویندوز                 |
| معماری       | Code-behind               | رویکرد کلاسیک UI و منطق در یک فایل |

---
