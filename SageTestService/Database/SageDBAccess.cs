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
            OdbcConnection todbc1 = new OdbcConnection(System.Configuration.ConfigurationManager.AppSettings["OdbcConnectionString"]);

            try
            {
                Log.Information("SageDBAccess connection string: " + todbc1.ConnectionString.ToString());
                Log.Information("Connection state before open: " + todbc1.State);
                Log.Information("Try catch block added for open connection");
                try
                {
                    string connectionString =System.Configuration.ConfigurationManager.AppSettings["OdbcConnectionString"];
                    using var todbc = new OdbcConnection(connectionString);
                    //OdbcConnection todbc = new OdbcConnection(connectionString);
                   // Log.Information("SageDBAccess connection string: " + todbc.ConnectionString.ToString());
                   var conopen=  todbc.OpenAsync().Exception.Message.ToString();
                    Log.Information("SageDBAccess connection string state: " + conopen);

                    //await todbc.OpenAsync();

                    Log.Information("Database connection State: " + todbc.State);

                if (todbc.State == System.Data.ConnectionState.Open)
                {
                   
                    Log.Information("Database connection opened successfully. State: " + todbc.State);
                }
                else
                {
                   
                    Log.Error("Database connection FAILED to open. State: " + todbc.State);
                    return $"Connection failed. State: {todbc.State}";
                }
                String? result = null;
                string jobId = " 268295-1"; 
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
                   
                    Log.Warning("No data found with this Job ID:  268295-1");
                    result = "no rows found";
                }

                return result ?? "no rows found";
                }
                catch (Exception ex)
                { Log.Information("Database Open exception error : " + ex.Message.ToString()); throw; }

            }
            catch (Exception ex)
            {               
                Log.Error("DB access exception: " + ex.Message);
                return ex.Message;
            }
            finally
            {
                if (todbc1.State == System.Data.ConnectionState.Open)
                {
                    todbc1.Close();                   
                    Log.Information("Database connection closed.");
                }
            }

        }
    }
}
