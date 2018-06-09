using System.Data.SqlClient;

namespace ProjetoCRUD
{
    public class UsuarioDAO
    {
        public void Adicionar(Usuario pessoa)
        {
            using (SqlConnection conn = SqlConn.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO USUARIO 
                                                        (NOME, EMAIL) 
                                                        VALUES (@nome,@email)", conn))
                {
                    cmd.Parameters.AddWithValue("@nome", pessoa.Nome);
                    cmd.Parameters.AddWithValue("@email", pessoa.Email);

                    cmd.ExecuteNonQuery();

                    // É possível alterar o código de status da resposta
                    //Response.StatusCode = 201;
                }
            }
        }

        public void Editar(Usuario pessoa)
        {
            using (SqlConnection conn = SqlConn.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@"UPDATE USUARIO 
                                                        SET NOME = @nome, EMAIL = @email WHERE ID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@nome", pessoa.Nome);
                    cmd.Parameters.AddWithValue("@email", pessoa.Email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool Excluir(int id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(@" DELETE FROM USUARIO
                                                         WHERE ID = @id", conn))

                {
                    cmd.Parameters.AddWithValue("@id", id);

                    return (cmd.ExecuteNonQuery() == 1);
                }
            }
        }
    }
}