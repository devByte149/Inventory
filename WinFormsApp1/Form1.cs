using Microsoft.Data.Sqlite;
using System.Data;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadInventory();
        }
        
        private void LoadInventory()
        {
            using var connection = new SqliteConnection("Data Source=inventory.db");
            connection.Open();

            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Quantity", typeof(int));

            using var command = new SqliteCommand("SELECT * FROM Items", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                table.Rows.Add(
                    reader.GetInt32(0),         // Id
                    reader.GetString(1),        // Name
                    reader.GetInt32(2)          // Quantity
                );
            }

            dataGridView1.DataSource = table;
            dataGridView1.Columns["Id"].Visible = false; // Hide ID column
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection("Data Source=inventory.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Items (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Quantity INTEGER NOT NULL
        );
    ";
            command.ExecuteNonQuery();
        }
    }
}
