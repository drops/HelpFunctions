using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpFunctions
{
    public class DataBaseParameter
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string DataBaseName { get; set; }
        public string Catalog { get; set; }
        public bool IntegratedSecurity { get; set; }
        public bool IsProcedure { get; set; }

        public DataBaseParameter(string login, string password, string database, string catalog, bool integratedsecurity)
        {
            Login = login;
            Password = password;
            DataBaseName = database;
            Catalog = catalog;
            IntegratedSecurity = integratedsecurity;
            IsProcedure = false;
        }
    }
}
