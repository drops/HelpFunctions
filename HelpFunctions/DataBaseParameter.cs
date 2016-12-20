using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpFunctions
{
    public class DataBaseParameter
    {
        public string Login = "";
        public string Password = "";
        public string DataBaseName = "";
        public string Catalog = "";
        public bool IntegratedSecurity = false;

        public DataBaseParameter(string login, string password, string database, string catalog, bool integratedsecurity)
        {
            Login = login;
            Password = password;
            DataBaseName = database;
            Catalog = catalog;
            IntegratedSecurity = integratedsecurity;
        }
    }
}
