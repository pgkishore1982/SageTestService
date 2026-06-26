using Serilog;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
namespace SageTestService.Database
{
 
    public class SageDBAccess
    {       
        public static async Task<String> OpenJob()
        {
            var todbc = DbConnection.GetOdbcConnection();

            try
            {
                Log.Information("SageDBAccess connection string: " + todbc.ConnectionString.ToString());
                todbc.Open();
                Log.Information("Database connection State: " + todbc.State);
                String? result = null;

                string jobId = "YOUR_JOB_ID"; 
                string checkSql = $"SELECT Status FROM MASTER_JCM_JOB_1 WHERE Job = '{jobId}'";
                Log.Information("Inline query: " + checkSql);


                var cmd = new OdbcCommand(checkSql, todbc);
                var reader = await cmd.ExecuteReaderAsync();
                Log.Information("Data reader HasRows: " + reader.HasRows);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        string status = reader["Status"]?.ToString() ?? string.Empty;
                        Log.Information("Record found - Status: " + status);
                        result = status;
                    }
                }
                else
                {
                   
                    Log.Warning("No data found with this Job ID.");
                    result = "no rows found";
                }

                return result ?? "no rows found";
            }
            catch (Exception ex)
            {               
                Log.Error("DB access exception: " + ex.Message);
                return ex.Message;
            }
            finally
            {
                if (todbc.State == System.Data.ConnectionState.Open)
                {
                    todbc.Close();                   
                    Log.Information("Database connection closed.");
                }
            }

        }
    }
}
