# 📚 راهنمای شخصی پروژه TodoAppSQLite

---

## 1. Program.cs

```csharp
using System;
using System.Windows.Forms;

namespace TodoAppSQLite
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
```

**توضیح:**
- `using System;` و `using System.Windows.Forms;`: اضافه کردن فضای نام‌های مورد نیاز.
- `namespace TodoAppSQLite`: تعریف فضای نام پروژه.
- `static class Program`: کلاس اصلی برنامه.
- `[STAThread]`: تعیین مدل threading برای فرم‌ها (ضروری برای WinForms).
- `Main()`: نقطه شروع برنامه.
    - `EnableVisualStyles()`: فعال‌سازی ظاهر مدرن ویندوز.
    - `SetCompatibleTextRenderingDefault(false)`: تنظیم رندرینگ متن.
    - `Application.Run(new MainForm())`: اجرای فرم اصلی برنامه.

---

## 2. MainForm.Designer.cs

این فایل فقط طراحی و چیدمان کنترل‌های فرم را انجام می‌دهد.

```csharp
namespace TodoAppSQLite
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox taskInput;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button toggleButton;
        private System.Windows.Forms.ListBox taskList;
        private System.Windows.Forms.DateTimePicker dueDatePicker;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.taskInput = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.toggleButton = new System.Windows.Forms.Button();
            this.taskList = new System.Windows.Forms.ListBox();
            this.dueDatePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();

            // taskInput
            this.taskInput.Location = new System.Drawing.Point(12, 12);
            this.taskInput.Size = new System.Drawing.Size(300, 23);

            // dueDatePicker
            this.dueDatePicker.Location = new System.Drawing.Point(12, 41);
            this.dueDatePicker.Size = new System.Drawing.Size(300, 23);

            // addButton
            this.addButton.Location = new System.Drawing.Point(318, 12);
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);

            // removeButton
            this.removeButton.Location = new System.Drawing.Point(318, 41);
            this.removeButton.Text = "Remove";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);

            // toggleButton
            this.toggleButton.Location = new System.Drawing.Point(318, 70);
            this.toggleButton.Text = "Toggle Status";
            this.toggleButton.Click += new System.EventHandler(this.toggleButton_Click);

            // taskList
            this.taskList.Location = new System.Drawing.Point(12, 70);
            this.taskList.Size = new System.Drawing.Size(300, 180);

            // MainForm
            this.ClientSize = new System.Drawing.Size(420, 260);
            this.Controls.Add(this.taskInput);
            this.Controls.Add(this.dueDatePicker);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.taskList);
            this.Text = "To-Do List with SQLite";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
```

**توضیح:**
- تعریف کنترل‌های فرم (TextBox، Button، ListBox، DateTimePicker).
- تعیین موقعیت و اندازه هر کنترل.
- اتصال رویداد کلیک دکمه‌ها به متدهای مربوطه.
- اضافه کردن کنترل‌ها به فرم.

---

## 3. MainForm.cs

```csharp
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace TodoAppSQLite
{
    public partial class MainForm : Form
    {
        private const string DbFile = "tasks.db";
        private SQLiteConnection connection;

        public MainForm()
        {
            InitializeComponent();
            InitDatabase();
            LoadTasks();
        }

        private void InitDatabase()
        {
            if (!File.Exists(DbFile))
            {
                SQLiteConnection.CreateFile(DbFile);
            }

            connection = new SQLiteConnection("Data Source=" + DbFile);
            connection.Open();

            string tableCmd = @"CREATE TABLE IF NOT EXISTS Tasks (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Title TEXT NOT NULL,
                                    IsCompleted INTEGER NOT NULL,
                                    DueDate TEXT
                                );";
            new SQLiteCommand(tableCmd, connection).ExecuteNonQuery();
        }

        private void LoadTasks()
        {
            taskList.Items.Clear();
            var cmd = new SQLiteCommand("SELECT Id, Title, IsCompleted, DueDate FROM Tasks", connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string status = reader.GetInt32(2) == 1 ? "[Done] " : "[Pending] ";
                string dueDate = reader["DueDate"]?.ToString();
                string display = $"{status}{reader["Title"]} (Due: {dueDate})";
                taskList.Items.Add(display);
            }

            reader.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(taskInput.Text))
            {
                var due = dueDatePicker.Value.ToString("yyyy-MM-dd");
                var cmd = new SQLiteCommand("INSERT INTO Tasks (Title, IsCompleted, DueDate) VALUES (@title, 0, @due)", connection);
                cmd.Parameters.AddWithValue("@title", taskInput.Text);
                cmd.Parameters.AddWithValue("@due", due);
                cmd.ExecuteNonQuery();
                taskInput.Clear();
                LoadTasks();
            }
        }

        private void toggleButton_Click(object sender, EventArgs e)
        {
            int index = taskList.SelectedIndex;
            if (index < 0) return;

            var cmdGet = new SQLiteCommand("SELECT Id, IsCompleted FROM Tasks", connection);
            var reader = cmdGet.ExecuteReader();

            int i = 0, taskId = -1, currentStatus = 0;
            while (reader.Read())
            {
                if (i == index)
                {
                    taskId = reader.GetInt32(0);
                    currentStatus = reader.GetInt32(1);
                    break;
                }
                i++;
            }

            reader.Close();

            if (taskId != -1)
            {
                var cmd = new SQLiteCommand("UPDATE Tasks SET IsCompleted = @status WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("@status", currentStatus == 0 ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", taskId);
                cmd.ExecuteNonQuery();
                LoadTasks();
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            int index = taskList.SelectedIndex;
            if (index < 0) return;

            var cmdGet = new SQLiteCommand("SELECT Id FROM Tasks", connection);
            var reader = cmdGet.ExecuteReader();

            int i = 0, taskId = -1;
            while (reader.Read())
            {
                if (i == index)
                {
                    taskId = reader.GetInt32(0);
                    break;
                }
                i++;
            }

            reader.Close();

            if (taskId != -1)
            {
                var cmd = new SQLiteCommand("DELETE FROM Tasks WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("@id", taskId);
                cmd.ExecuteNonQuery();
                LoadTasks();
            }
        }
    }
}
```

**توضیح:**
- `DbFile`: نام فایل دیتابیس.
- `connection`: شیء اتصال به دیتابیس.
- `MainForm()`: سازنده فرم، مقداردهی اولیه و بارگذاری داده‌ها.
- `InitDatabase()`: اگر دیتابیس وجود ندارد، ایجادش می‌کند و جدول `Tasks` را می‌سازد.
- `LoadTasks()`: همه تسک‌ها را از دیتابیس می‌خواند و در لیست نمایش می‌دهد.
- `addButton_Click`: اگر ورودی خالی نبود، تسک جدید را با تاریخ انتخاب‌شده اضافه می‌کند.
- `toggleButton_Click`: وضعیت انجام‌شدن تسک انتخاب‌شده را برعکس می‌کند (انجام‌شده/نشده).
- `removeButton_Click`: تسک انتخاب‌شده را حذف می‌کند.

---

## 4. TodoAppSQLite.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net6.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="System.Data.SQLite" Version="1.0.119" />
    <PackageReference Include="System.Data.SQLite.Core" Version="1.0.119" />
  </ItemGroup>

</Project>
```

**توضیح:**
- پروژه از نوع WinExe (برنامه گرافیکی ویندوز).
- هدف فریم‌ورک: net6.0-windows.
- استفاده از Windows Forms.
- اضافه کردن پکیج‌های SQLite برای ارتباط با دیتابیس.

---

## 5. .gitignore

```
bin/
obj/
.vs/
*.user
*.suo
*.db
*.exe
*.cache
*.log
```

**توضیح:**
- پوشه‌ها و فایل‌هایی که نباید وارد گیت شوند (فایل‌های خروجی، دیتابیس، تنظیمات شخصی و ...).

---

## 6. README.md و README_ANALYSIS.md

- [README.md](README.md): توضیح کلی پروژه، امکانات، نحوه اجرا و اطلاعات نویسنده.
- [README_ANALYSIS.md](README_ANALYSIS.md): تحلیل فنی و جزئیات کدها و ساختار پروژه.

---

### نکات پایانی

- کدها کاملاً ساده و قابل فهم هستند.
- هر تابع و رویداد به صورت جداگانه و با منطق واضح پیاده‌سازی شده است.
- امنیت اولیه با استفاده از پارامترهای SQL رعایت شده.
