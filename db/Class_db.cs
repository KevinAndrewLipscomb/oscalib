using MySql.Data.MySqlClient;
using System.Collections;
using System.Configuration;
using System.Data;

namespace Class_db
  {

  public abstract class TClass_db
    {
    protected MySqlConnection Connection { get; set; } = null;

    public TClass_db() : base()
      {
      Connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["db_connection_string"].ConnectionString);
      }

    protected void Close()
      {
      Connection.Close();
      }

    protected void ExecuteOneOffProcedureScriptWithTolerance
      (
      string procedure_name,
      MySqlScript my_sql_script
      )
      {
      var done = false;
      while (!done)
        {
        try
          {
          my_sql_script.Execute();
          done = true;
          }
        catch (MySqlException the_exception)
          {
          if (!new ArrayList() {"PROCEDURE " + procedure_name + " already exists","PROCEDURE " + Connection.Database + "." + procedure_name + " does not exist"}.Contains(the_exception.Message))
            {
            throw;
            }
          }
        }
      }

    protected void Open()
      {
      if (Connection.State != ConnectionState.Open)
        {
        Connection.Open();
        (new MySqlCommand("set session sql_mode = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION'",Connection)).ExecuteNonQuery();
        }
      }

    } // end TClass_db

  }
