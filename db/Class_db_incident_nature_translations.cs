// Derived from KiAspdotnetFramework/component/db/Class~db~template~kicrudhelped~items.cs~template

using Class_db;
using Class_db_trail;
using kix;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System;

namespace Class_db_incident_nature_translations
  {
  public class TClass_db_incident_nature_translations: TClass_db
    {
    private class incident_nature_translation_summary
      {
      public string id;
      }

    private TClass_db_trail db_trail = null;

    public TClass_db_incident_nature_translations() : base()
      {
      db_trail = new TClass_db_trail();
      }

    public bool Bind(string partial_spec, object target)
      {
      var concat_clause = "concat(IFNULL(foreign,'-'),'|',IFNULL(local,'-'))";
      Open();
      ((target) as ListControl).Items.Clear();
      var dr = new MySqlCommand
        (
        "select id"
        + " , CONVERT(" + concat_clause + " USING utf8) as spec"
        + " from incident_nature_translation"
        + " where " + concat_clause + " like '%" + partial_spec.ToUpper() + "%'"
        + " order by spec",
        connection
        )
        .ExecuteReader();
      while (dr.Read())
        {
        ((target) as ListControl).Items.Add(new ListItem(dr["spec"].ToString(), dr["id"].ToString()));
        }
      dr.Close();
      Close();
      return ((target) as ListControl).Items.Count > 0;
      }

    internal string LocalOfForeign(string nature)
      {
      Open();
      var local_of_foreign_obj = new MySqlCommand("select local from incident_nature_translation where `foreign` = '" + nature + "'",connection).ExecuteScalar();
      Close();
      return (local_of_foreign_obj == null ? k.EMPTY : local_of_foreign_obj.ToString());
      }

    internal void BindBaseDataList
      (
      string sort_order,
      bool be_sort_order_ascending,
      object target
      )
      {
      Open();
      ((target) as BaseDataList).DataSource = new MySqlCommand
        (
        "select incident_nature_translation.id as id"
        + " from incident_nature_translation",
        connection
        )
        .ExecuteReader();
      ((target) as BaseDataList).DataBind();
      Close();
      }

    public void BindDirectToListControl(object target)
      {
      Open();
      ((target) as ListControl).Items.Clear();
      var dr = new MySqlCommand
        (
        "SELECT id"
        + " , CONVERT(concat(IFNULL(foreign,'-'),'|',IFNULL(local,'-')) USING utf8) as spec"
        + " FROM incident_nature_translation"
        + " order by spec",
        connection
        )
        .ExecuteReader();
      while (dr.Read())
        {
        ((target) as ListControl).Items.Add(new ListItem(dr["spec"].ToString(), dr["id"].ToString()));
        }
      dr.Close();
      Close();
      }

    public bool Delete(string id)
      {
      var result = true;
      Open();
      try
        {
        new MySqlCommand(db_trail.Saved("delete from incident_nature_translation where id = \"" + id + "\""), connection).ExecuteNonQuery();
        }
      catch(System.Exception e)
        {
        if (e.Message.StartsWith("Cannot delete or update a parent row: a foreign key constraint fails", true, null))
          {
          result = false;
          }
        else
          {
          throw e;
          }
        }
      Close();
      return result;
      }

    public bool Get
      (
      string id,
      out string foreign,
      out string local
      )
      {
      foreign = k.EMPTY;
      local = k.EMPTY;
      var result = false;
      //
      Open();
      var dr = new MySqlCommand("select * from incident_nature_translation where CAST(id AS CHAR) = \"" + id + "\"", connection).ExecuteReader();
      if (dr.Read())
        {
        foreign = dr["foreign"].ToString();
        local = dr["local"].ToString();
        result = true;
        }
      dr.Close();
      Close();
      return result;
      }

    public void Set
      (
      string id,
      string foreign,
      string local
      )
      {
      var childless_field_assignments_clause = k.EMPTY
      + "foreign = NULLIF('" + foreign + "','')"
      + " , local = NULLIF('" + local + "','')"
      + k.EMPTY;
      db_trail.MimicTraditionalInsertOnDuplicateKeyUpdate
        (
        target_table_name:"incident_nature_translation",
        key_field_name:"id",
        key_field_value:id,
        childless_field_assignments_clause:childless_field_assignments_clause
        );
      }

    internal object Summary(string id)
      {
      Open();
      var dr =
        (
        new MySqlCommand
          (
          "SELECT *"
          + " FROM incident_nature_translation"
          + " where id = '" + id + "'",
          connection
          )
          .ExecuteReader()
        );
      dr.Read();
      var the_summary = new incident_nature_translation_summary()
        {
        id = id
        };
      Close();
      return the_summary;
      }

    } // end TClass_db_incident_nature_translations

  }
