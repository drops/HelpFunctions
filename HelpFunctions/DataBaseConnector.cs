using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HelpFunctions
{
    public sealed class DataBaseConnector
    {
        private static DataBaseConnector Instance = null;

        string Login
        { get; set; }
        string Password
        { get; set; }
        string DataBaseName
        { get; set; }
        string Catalog
        { get; set; }

        private string ConnectionString;
        private string CommandString;
        bool IsProcedure = false;
        bool IntegratedSecurity = false;


        SqlConnection connection;
        SqlCommand command;

        private DataBaseConnector()
        {
            connection = new SqlConnection();
        }

        public static DataBaseConnector instance
        {
            get
            {
                if(Instance == null)
                {
                    Instance = new DataBaseConnector();
                }
                return Instance;
            }
        }

        public DataBaseConnector(string login, string password, string databasename, string catalog, bool isitegratedsecurity)
        {
            Login = login;
            Password = password;
            DataBaseName = databasename;
            Catalog = catalog;
            IntegratedSecurity = isitegratedsecurity;
        }


        private void InitializeCommand()
        {
            if(IsProcedure == true)
            {
                command = new SqlCommand(CommandString);
                command.CommandType = System.Data.CommandType.StoredProcedure;
            }
            else
            {
                command = new SqlCommand(CommandString);
            }
        }

        private void SetCommand(string commandString, bool isProcedureName)
        {
            CommandString = commandString;
            IsProcedure = isProcedureName;
        }

        private void Open()
        {
            if(connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        }

        private void Close()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }


        private void SetProcedure(string procedureNameString)
        {
            command = new SqlCommand(procedureNameString);
            command.CommandType = System.Data.CommandType.StoredProcedure;
        }


        private void GenerateConnectionString()
        {
            if(IntegratedSecurity == true)
            {
                ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", DataBaseName, Catalog);
            }else
            {
                ConnectionString = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", DataBaseName, Catalog, Login, Password);
            }
        }

    }
}
