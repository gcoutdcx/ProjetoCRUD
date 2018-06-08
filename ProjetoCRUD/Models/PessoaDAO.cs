using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ProjetoCRUD.BD;

namespace ProjetoCRUD.Models
{
    public class PessoaDAO
    {
        public void Adicionar(Pessoa pessoa)
        {
            using(SqlConnection conn = SqlConn.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO tbPessoa 
                                                        (nome, email) 
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

        public void Editar(Pessoa pessoa)
        {
            using (SqlConnection conn = SqlConn.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@"UPDATE tbPessoa 
                                                        SET nome = @nome, email = @email WHERE Id = @id", conn))
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

                using (SqlCommand cmd = new SqlCommand(@" DELETE FROM tbPessoa
                                                         WHERE UsuarioId = @id", conn))

                {
                    cmd.Parameters.AddWithValue("@id", id);

                    return (cmd.ExecuteNonQuery() == 1);
                }
            }
        }
    }
}