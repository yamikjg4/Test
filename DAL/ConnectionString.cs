using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class ConnectionString
    {
        public const string localdb = "Server=(LocalDB)\\Raone; Database=db_test; Trusted_Connection=True; Encrypt=False";
        public const string UAT = "Server=new; Database=db_test; Trusted_Connection=True; Encrypt=False";
        public const string Live = "Server=(LocalDB)\\Raone; Database=db_test; Trusted_Connection=True; Encrypt=False";
    }
}
