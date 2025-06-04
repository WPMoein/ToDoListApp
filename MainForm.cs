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