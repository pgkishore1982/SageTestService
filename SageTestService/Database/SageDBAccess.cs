using Serilog;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
namespace SageTestService.Database
{
 
    public class SageDBAccess
    {       
        public static async Task<String> OpenJob(string jobNo)
        {           
            using (var todbc = DbConnection.GetOdbcConnection())
            {
                try
                {
                    Log.Information("SageDBAccess connection string: "+ todbc.ConnectionString.ToString());
                    await todbc.OpenAsync();
                    String? result = null;
                    if (!string.IsNullOrEmpty(jobNo))
                    {
                        Log.Information("Open dataBase connection : "+ "Successs");
                        string checkSql = $"SELECT Status FROM MASTER_JCM_JOB_1 WHERE LOWER(Status) = 'closed' AND Job = ?";
                        using (var cmd = new OdbcCommand(checkSql, todbc))
                        {
                            cmd.Parameters.AddWithValue("@Job", jobNo);
                            var reader = await cmd.ExecuteReaderAsync();
                            Log.Information("data reader :" + reader.HasRows);
                            if (reader.HasRows)
                            {
                                
                                string updateSql = "UPDATE MASTER_JCM_JOB_1 SET Status = 'inprogress' WHERE Job = ?";
                                using (var updateCmd = new OdbcCommand(updateSql, todbc))
                                {
                                    updateCmd.Parameters.AddWithValue("@Job", jobNo);
                                   var r= await updateCmd.ExecuteNonQueryAsync();
                                    Log.Information("update result :" + r.ToString());
                                    result =r.ToString();
                                }
                            }
                            else
                            {
                                result = "no rows found";
                            }
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Log.Error("DB access issue : "+ ex.Message.ToString());
                    string exception=ex.Message.ToString();
                    return exception;                   
                }
            }
        }
    }
}
