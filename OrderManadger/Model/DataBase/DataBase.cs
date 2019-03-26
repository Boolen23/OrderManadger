using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManadger.Model.DataBase
{
    public class DataBase
    {
        public DataBase()
        {
            FirstStart();
        }
        public event EventHandler<DataBaseEventArgs> StatusChanged;
       string connStr = @"Data Source=.\SQLEXPRESS; Initial Catalog=T; Integrated Security=True; database=master";
       // string connStr = @"Server='C:\\MyFolder\\New1.mdf';Integrated security=True;database=master";


        private SqlConnection conn;
        public async Task<bool> TryConnectAsync(string connect)
        {
            conn = new SqlConnection(connect);
            try
            {
                await conn.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async void FirstStart()
        {
            Status = "Попытка подключения...";
            if (await TryConnectAsync(connStr))
            {
                Status = "Подключение установлено";
            }
            else
            {
                Status = "База данных не найдена или повреждена." + Environment.NewLine + "Создание базы данных...";
               await CreateBase();
                await CreateTables();
                Status = "База данных создана!";
            }
        }
        private async Task CreateBase()
        {
            conn.Close();

            //conn = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Integrated Security=True");
            //SqlCommand cmdCreateDataBase = new SqlCommand(string.Format("CREATE DATABASE [{0}]", "nio1234567890"), conn);
            String str;
            conn = new SqlConnection("Server=localhost;Integrated security=True;database=master");

            str = "CREATE DATABASE T ON PRIMARY " +
                "(NAME = T1, " +
                "FILENAME = 'C:\\MyFolder\\T1.mdf', " +
                "SIZE = 2MB)";

            SqlCommand myCommand = new SqlCommand(str, conn);



           await conn.OpenAsync();
           await  myCommand.ExecuteNonQueryAsync();
            await Task.Delay(2000);
            conn.Close();
            await Task.Delay(5000);
            conn = new SqlConnection(connStr);
           await conn.OpenAsync();
        }
        private async Task CreateTables()
        {
            string text = @"CREATE TABLE Entry(
                        ID int IDENTITY(1,1) PRIMARY KEY,
                        OrderList varchar(max) NOT NULL,
                        Date datetime,
                        Comment varchar(255),
                        Status varchar (20) );";
            SqlCommand cmdCreateTable = new SqlCommand(text, conn);
            await cmdCreateTable.ExecuteNonQueryAsync();
            await Task.Delay(100);

            text = @"CREATE TABLE Sellers(
                        ID int IDENTITY(1,1) PRIMARY KEY,
                        Seller varchar(255) NOT NULL );";
            cmdCreateTable = new SqlCommand(text, conn);
            await cmdCreateTable.ExecuteNonQueryAsync();
            await Task.Delay(100);

            text = @"CREATE TABLE Assortment(
                        ID int IDENTITY(1,1) PRIMARY KEY,
                        Position varchar(255) NOT NULL );";
            cmdCreateTable = new SqlCommand(text, conn);
            await cmdCreateTable.ExecuteNonQueryAsync();
            await Task.Delay(100);
        }
        public string Status
        {
            set => StatusChanged?.Invoke(null, new DataBaseEventArgs(value));
        }

        public async void Test()
        {
            SqlCommand cmd = new SqlCommand(@"show databases;",conn);
            await cmd.ExecuteNonQueryAsync();


            //SqlCommand cmd = new SqlCommand("INSERT INTO Entry VALUES ('seg','@time', 'srags', 'seGF')",conn);
            //cmd.Parameters.AddWithValue("@time", DateTime.Now);
            //await cmd.ExecuteNonQueryAsync();
            //await Task.Delay(100);

            //cmd = new SqlCommand("INSERT INTO Sellers VALUES ('1seg')", conn);
            //await cmd.ExecuteNonQueryAsync();
            //await Task.Delay(100);

            //cmd = new SqlCommand("INSERT INTO Assortment VALUES ('2seg')", conn);
            //await cmd.ExecuteNonQueryAsync();
            //await Task.Delay(100);
            //Status = "Запись в базу данных произведена успешно";
        }
    }
}
