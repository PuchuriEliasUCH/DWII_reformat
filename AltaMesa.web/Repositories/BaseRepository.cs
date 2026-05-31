using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AltaMesa.web.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        protected BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AltaMesaDB"].ConnectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected SqlCommand GetCommand(SqlConnection connection, string procedureName)
        {
            var cmd = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            return cmd;
        }
    }
}
