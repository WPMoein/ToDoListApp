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