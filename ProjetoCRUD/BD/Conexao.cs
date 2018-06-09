using System.Data.SqlClient;

namespace ProjetoCRUD
{
    public static class Conexao
    {
        public static SqlConnection Abrir()
        {
            string str = System.Configuration.ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            return conn;
        }
    }
}