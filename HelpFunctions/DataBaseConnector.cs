using System;
using System.Data;
using System.Data.SqlClient;

namespace HelpFunctions
{
    public class DataBaseConnector : IDisposable
    {
        private bool m_bIsDisposed = false;

        DataBaseParameter Parameters;
        DataBaseCredential Credential;

        private string ConnectionString;
        private string CommandString;
        bool IsProcedure = false;
        bool IntegratedSecurity = false;


        SqlConnection connection;
        SqlCommand command;
        DataTable ResultTable;
        ErrorLog errorLog;
        int ErrorLogMode = 0;  // 0 - zapis do pliku, 
                               // 1 - zapis do pliku i komunikat w konsoli,
                               // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
                               // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym
        


    

        public DataBaseConnector()
        {
            errorLog = new ErrorLog();
            connection = new SqlConnection();
        }

        public DataBaseConnector(DataBaseParameter parameter)
        {
            errorLog = new ErrorLog();
            Parameters = parameter;
            GenerateConnectionString();
            connection = new SqlConnection(ConnectionString);
            IntegratedSecurity = true;
        }

        public DataBaseConnector(DataBaseParameter parameters, DataBaseCredential credential)
        {
            errorLog = new ErrorLog();
            Parameters = parameters;
            Credential = credential;
            GenerateConnectionString();
            connection = new SqlConnection(ConnectionString);
        }

        public DataBaseConnector(string path, string filename)
        {
            errorLog = new ErrorLog(path, filename);
        }

        private void InitializeCommand()
        {
            if(IsProcedure == true)
            {
                command = new SqlCommand(CommandString, connection);
                command.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                command = new SqlCommand(CommandString, connection);
            }
        }

        public void AddConnectionParameterAndCredential(DataBaseParameter parameter, DataBaseCredential credential)
        {
            Parameters = parameter;
            Credential = credential;
            GenerateConnectionString();
            connection = new SqlConnection(ConnectionString);
        }

        

        /// <summary>
        // 0 - zapis do pliku, 
        // 1 - zapis do pliku i komunikat w konsoli,
        // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
        // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym
        /// </summary>
        /// <param name="mode"></param>
        public void SetErrorLogMode(int mode)
        {
            ErrorLogMode = mode;
        }

        public bool Open()
       { 
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return true;
            }catch(SqlException e)
            {
                SaveError("HelpFunctions->DataBaseConnector->Open: " + e.ToString());
                return false;
            }   
        }

        public bool Close()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                return true;
            }catch (SqlException e)
            {
                SaveError("HelpFunctions->DataBaseConnector->Close: " + e.ToString());
                return false;
            }
        }


        public void SetCommand(string commandString, bool isProcedureName)
        {
            CommandString = commandString;
            IsProcedure = isProcedureName;
            InitializeCommand();
        }

        public void SetCommandTimeOut(int seconds)
        {
            if (command != null)
                command.CommandTimeout = seconds;
        }

        public void SetProcedure(string procedureNameString)
        {
            command = new SqlCommand(procedureNameString, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
        }


        public void AddParametersToCommand(CommandParameter parameters)
        {
            for(int i = 0; i < parameters.ParameterCount(); i++)
            {
                command.Parameters.Add(parameters.ReturnSingleParameterByPosition(i));
            }
        }

        public void CleanParameters()
        {
            command.Parameters.Clear();
        }

        public int Execute()
        {
            try
            {
                if (connection.State == ConnectionState.Closed) return -99;
                if (command == null) return -100;
                int result = command.ExecuteNonQuery();
                return result;
            }catch (SqlException e)
            {
                SaveError("HelpFunctions->DataBaseConnector->Execute: " + e.ToString());
                if (connection.State == ConnectionState.Open) Close();
                return -100;
            }
            
        }

        public int CompleteExecute()
        {
            try
            {
                Open();
                if (command == null) return -100;
                int result = command.ExecuteNonQuery();
                Close();
                return result;
            }catch (SqlException e)
            {
                SaveError("HelpFunctions->DataBaseConnector->CompleteExecute: " + e.ToString());
                if (connection.State == ConnectionState.Open) Close();
                return -99;
            }
        }

        public int ExecuteAndSaveToTable()
        {
            try
            {
                if (connection.State == ConnectionState.Closed) return -99;
                if (command == null) return -100;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                ResultTable = new DataTable();
                int result = adapter.Fill(ResultTable);
                return result;
            }catch (SqlException e)
            {
                SaveError("HelpFunctions->DataBaseConnector->ExecuteAndSaveToTable: " + e.ToString());
                if (connection.State == ConnectionState.Open) Close();
                return -100;
            }
        }

        public int CompleteExecuteAndSaveToTable()
        {
            try
            {
                Open();
                if (command == null) return -100;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                ResultTable = new DataTable();
                int result = adapter.Fill(ResultTable);
                Close();
                return result;
            }catch (SqlException e)
            {
                SaveError("HelpFunctions->DataBaseConnector->CompleteExecuteAndSaveToTable: " + e.ToString());
                if (connection.State == ConnectionState.Open) Close();
                return -99;
            }
        }

        public DataTable ReturnResultTable()
        {
            return ResultTable;
        }


        private void GenerateConnectionString()
        {
            if(IntegratedSecurity == true || Credential == null)
            {
                ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", Parameters.DataBaseName, Parameters.Catalog);
            }else
            {
                ConnectionString = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", Parameters.DataBaseName, Parameters.Catalog, Credential.Login, Credential.Password);
            }
        }

        private string SaveError(string ErrorMessage)
        {
            if (ErrorLogMode == 0) errorLog.WriteLog(ErrorMessage);
            else if (ErrorLogMode == 1) errorLog.WriteAndShowLog(ErrorMessage);
            else if (ErrorLogMode == 2) errorLog.WriteToLogAndTell(ErrorMessage);
            else if (ErrorLogMode == 3) errorLog.WriteAndShowWindowLog(ErrorMessage);
            else errorLog.WriteLog(ErrorMessage);
            return ErrorMessage;
        }

        public void ChangeErrorLogFileName(string filename)
        {
            errorLog.ChangeFileName(filename);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataBaseConnector()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool bDisposing)
        {
            if(m_bIsDisposed)
            {
                return;
            }
            if(bDisposing)
            {
                //zwalnianie zarządzanych zasobów
            }
            //zwalnianie niezarządzanych zasobów
            m_bIsDisposed = true;
        }
    }
}
