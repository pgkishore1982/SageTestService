using System.Data.Odbc;
using System.Configuration;
using Serilog;

namespace SageTestService.Database
{
    public class DbConnection
    {     
        public static OdbcConnection GetOdbcConnection()
        {
           OdbcConnection odbcConnection= new OdbcConnection(System.Configuration.ConfigurationManager.AppSettings["OdbcConnectionString"]);
            Log.Information("Sage DbConnection class: "+ odbcConnection.ConnectionString.ToString());
            return odbcConnection;
        }
    }
}
