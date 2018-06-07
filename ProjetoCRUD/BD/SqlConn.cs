using System.Data.SqlClient;


namespace ProjetoCRUD.BD
{
    public static class SqlConn
    {
        public static SqlConnection Abrir()
        {
            string str = System.Configuration.ConfigurationManager.ConnectionStrings["MinhaConn"].ConnectionString;
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            return conn;
        }
    }
}