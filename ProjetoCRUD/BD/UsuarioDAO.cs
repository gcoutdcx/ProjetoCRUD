using ProjetoCRUD.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjetoCRUD
{
    public class UsuarioDAO
    {
        public void Adicionar(Usuario usuario)
        {
            using (SqlConnection conn = Conexao.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO USUARIO 
                                                        (NOME, EMAIL) 
                                                        OUTPUT INSERTED.Id VALUES (@nome,@email)", conn))
                {
                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);

                    int id = (int)cmd.ExecuteScalar();
                    // É possível alterar o código de status da resposta
                    //Response.StatusCode = 201;
                }
            }
            
        }

        public void Editar(Usuario usuario)
        {
            using (SqlConnection conn = Conexao.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@"UPDATE USUARIO 
                                                        SET NOME = @nome, EMAIL = @email WHERE ID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModalEditar(int id)
        {
            Usuario usuario;
            using (SqlConnection conn = Conexao.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Nome, Email FROM USUARIO WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Obtém os registros, um por vez
                        if (reader.Read() == true)
                        {
                            string nome = reader.GetString(1);
                            string email = reader.GetString(2);

                            usuario = new Usuario();
                            usuario.Id = id;
                            usuario.Nome = nome;
                            usuario.Email = email;
                        }
                        else
                        {
                            usuario = null;
                        }
                    }
                }
            }
        }

        public bool Excluir(int id)
        {
            using (SqlConnection conn = Conexao.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@" DELETE FROM USUARIO
                                                         WHERE ID = @id", conn))

                {
                    cmd.Parameters.AddWithValue("@id", id);

                    return (cmd.ExecuteNonQuery() == 1);
                }
            }
        }

        public Usuario Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection conn = Conexao.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Nome, Email FROM USUARIO ORDER BY Nome ASC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Obtém os registros, um por vez
                        while (reader.Read() == true)
                        {
                            int id = reader.GetInt32(0);
                            string nome = reader.GetString(1);
                            string email = reader.GetString(2);

                            Usuario usuario = new Usuario();
                            usuario.Id = id;
                            usuario.Nome = nome;
                            usuario.Email = email;

                            lista.Add(usuario);
                            
                        }
                    }
                }
            }
            return null;
            
        }

    }
}