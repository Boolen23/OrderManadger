using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model.DataBase
{
    public partial class DataBase
    {
        public DataBase()
        {

        }
        public event EventHandler<DataBaseEventArgs> StatusChanged; 
        string connStr = @"Data Source=(local)\SQLEXPRESS; Initial Catalog=Base; Integrated Security=True";
        SqlConnection conn;
        public async Task<bool> TryConnectAsync(string connect)
        {
            conn = new SqlConnection(connect);
            try
            {
                await conn.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async void FirstStart()
        {
            Status = "Попытка подключения...";
            if(await TryConnectAsync(connStr))
            {
                Status = "Подключение установлено";
            }
        }
        private async Task CreateBase()
        {
            if (conn != null) conn.Close();
            conn = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Integrated Security=True");
            SqlCommand cmdCreateDataBase = new SqlCommand(string.Format("CREATE DATABASE [{0}]", "Base"), conn);
            await conn.OpenAsync();
            await cmdCreateDataBase.ExecuteNonQueryAsync();
            conn.Close();
            await Task.Delay(5000);
            conn = new SqlConnection(connStr);
            await conn.OpenAsync();
            string text = @"CREATE TABLE Entry(
                        ID int IDENTITY(1,1) PRIMARY KEY,
                        OrderList varchar(max) NOT NULL,
                        Date datetime,
                        Comment varchar(255),
                        Status varchar (20) );";
            SqlCommand cmdCreateTable = new SqlCommand(text, conn);

        }
        public string Status
        {
            set => StatusChanged?.Invoke(null, new DataBaseEventArgs(value));
        }
    }
}
