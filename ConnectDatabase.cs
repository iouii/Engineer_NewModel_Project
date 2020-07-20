using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace NewModelEx_S1
{
    public class ConnectDatabase
    {
        public SqlConnection con;
        public void OpenConnectionSql()
        {

            string constring = ConfigurationManager.ConnectionStrings["RConnection"].ConnectionString;
            con = new SqlConnection(constring);
            con.Open();
        }
        public void CloseConnectionSql()
        {
            con.Close();
        }
    }
}

