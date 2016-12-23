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

        DataBaseParameter Parameters;

        private string ConnectionString;
        private string CommandString;
        bool IsProcedure = false;
        bool IntegratedSecurity = false;


        SqlConnection connection;
        SqlCommand command;
        CommandParameter ComParameters;

        private DataBaseConnector()
        {
            connection = new SqlConnection();
        }

        public DataBaseConnector(DataBaseParameter parameter)
        {
            Parameters = parameter;
            GenerateConnectionString();
            connection = new SqlConnection(ConnectionString);
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


        public bool Open()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                return true;
            }catch(Exception e)
            {
                return false;
            }
            
        }

        public bool Close()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return true;
            }catch (Exception e)
            {
                return false;
            }
        }


        private void SetCommand(string commandString, bool isProcedureName)
        {
            CommandString = commandString;
            IsProcedure = isProcedureName;
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
                ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", Parameters.DataBaseName, Parameters.Catalog);
            }else
            {
                ConnectionString = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", Parameters.DataBaseName, Parameters.Catalog, Parameters.Login, Parameters.Password);
            }
        }

    }
}
