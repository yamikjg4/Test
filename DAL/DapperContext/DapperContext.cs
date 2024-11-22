using Microsoft.Data.SqlClient;
using System.Data;


namespace DAL.DapperContext
{

    public class DapperContext
    {
        /*private readonly string ConnectionString = "Server=(LocalDB)\\Raone; Database=db_test; Trusted_Connection=True; Encrypt=False";
        */
        public IDbConnection GetConnection() => new SqlConnection(ConnectionString.localdb);
    }

}