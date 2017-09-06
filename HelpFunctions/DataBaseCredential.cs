using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpFunctions
{
    public class DataBaseCredential
    {
        private string _login;
        public string Login
        {
            get
            {
                return _login;
            }
            private set
            {
                _login = value;
            }
        }
        string _password { get; set; }
        public string Password
        {
            get
            {
                return _password;
            }
            private set
            {
                _password = value;
            }
        }


        public DataBaseCredential(string login, string password)
        {
            Login = login;
            Password = password;
        }

    }
}
