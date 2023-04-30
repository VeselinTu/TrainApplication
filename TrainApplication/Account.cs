using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApplication
{
    public class Account
    {
        public string id;
        public string username ;
        public string password;

        public Account(string id, string username, string password)
        {
            this.id = id;
            this.username = username;
            this.password = password;
        }
    }
}