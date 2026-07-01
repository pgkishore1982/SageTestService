using Serilog;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace SageTestService.Database
{
 
    public class SageDBAccess
    {       
        public static async Task OpenJob()
        {

            try
            {

                Log.Information("Try catch block added for open connection");

                string? result = null;
                string jobId = "268295-1";

                string connectionString = System.Configuration.ConfigurationManager.AppSettings["OdbcConnectionString"]
                    ?? throw new InvalidOperationException("OdbcConnectionString not found in config.");

                 using var connection = new OdbcConnection(connectionString);
               
                Log.Information("Opening ODBC connection...");
                if (connection != null)
                {
                    connection.Open();
                    Log.Information("ODBC connection opened successfully." + connection.State);
                }
                else
                {
                    Log.Information("ODBC connection is null." + connection.Driver.ToString());
                }
                    const string sql = "SELECT Status FROM MASTER_JCM_JOB_1 WHERE Job ='268295-1'";

                 using var cmd = new OdbcCommand(sql, connection);
                //cmd.Parameters.Add("@jobId", OdbcType.VarChar).Value = jobId;

                 using var reader =  cmd.ExecuteReaderAsync();

                if (reader.Result.Read())
                {
                    var status = reader.Result["Status"]?.ToString();
                    Log.Information("Job {JobId} status: {Status}", jobId, status);
                }
                else
                {
                    Log.Warning("No job found for JobId: {JobId}", jobId);
                }

            }
            catch (OdbcException ex)
            {
                Log.Error("Failed to connect ODBC. Error: " + ex );
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("DB access exception: " + ex);
                throw;
            }
            
        }
    }
}
