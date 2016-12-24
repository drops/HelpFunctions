using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HelpFunctions
{
    public class CommandParameter
    {
        SqlParameter parameter;

        static List<SqlParameter> Parameters;

        public CommandParameter()
        {
            parameter = new SqlParameter();
            Parameters = new List<SqlParameter>();
        }

        public void AddParameter(SqlParameter param)
        {
            parameter = param;
            AddParameterToList(parameter);
        }

        public void AddParameterToList(SqlParameter parameter)
        {
            Parameters.Add(parameter);
        }

        public bool SetDataType(string parameterName, System.Data.DbType type)
        {
            for(int i = 0; i < Parameters.Count; i++)
            {
                if(Parameters[i].ParameterName == parameterName)
                {
                    Parameters[i].DbType = type;
                    return true;
                }
            }
            return false;
        }

        public SqlParameter ReturnSingleParameterByName(string parameterName)
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i].ParameterName == parameterName)
                {
                    return Parameters[i];
                }
            }
            return null;
        }

        public SqlParameter ReturnSingleParameterByPosition(int position)
        {
            return Parameters[position];
        }

        public int ParameterCount()
        {
            return Parameters.Count;
        }
    }
}
