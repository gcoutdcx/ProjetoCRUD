using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using ProjetoCRUD.Models;

namespace ProjetoCRUD
{
    public class UsuarioController : Controller
    {
        UsuarioDAO user = new UsuarioDAO();
        // GET: Pedido
        public ActionResult Index()
        {
            List<Usuario> lista = new List<Usuario>();

            // Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            // Veja mais strings de conexão em http://www.connectionstrings.com/
            using (SqlConnection conn = Conexao.Abrir())
            {
                // Cria um comando para selecionar registros da tabela, trazendo todas as pessoas que nasceram depois de 1/1/1900
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

            return View(lista);
        }

        public ActionResult Criar(Usuario usuario)
        {
            // Validar!!!!
            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                return Json("Nome inválido!");
            }

            // Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            // Veja mais strings de conexão em http://www.connectionstrings.com/
            using (SqlConnection conn = Conexao.Abrir())
            {
                // Cria um comando para inserir um novo registro à tabela
                using (SqlCommand cmd = new SqlCommand("INSERT INTO USUARIO (Nome, Email) OUTPUT INSERTED.Id VALUES (@nome, @email)", conn))
                {
                    // Esses valores poderiam vir de qualquer outro lugar, como uma TextBox...
                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);

                    int id = (int)cmd.ExecuteScalar();
                }
            }

            return Json("ok");
        }

        public ActionResult ModalEditar(int id)
        {
            //Pedido pedido;

            //// Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            //// Veja mais strings de conexão em http://www.connectionstrings.com/
            //using (SqlConnection conn = new SqlConnection(""))
            //{
            //    conn.Open();

            //    // Cria um comando para selecionar registros da tabela, trazendo todas as pessoas que nasceram depois de 1/1/1900
            //    using (SqlCommand cmd = new SqlCommand("SELECT Id, Nome, Valor FROM tbPedido WHERE Id = @id", conn))
            //    {
            //        cmd.Parameters.AddWithValue("@id", id);

            //        using (SqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            //Obtém os registros, um por vez
            //            if (reader.Read() == true)
            //            {
            //                string nome = reader.GetString(1);
            //                double valor = reader.GetDouble(2);

            //                pedido = new Pedido();
            //                pedido.Id = id;
            //                pedido.Nome = nome;
            //                pedido.Valor = valor;
            //            }
            //            else
            //            {
            //                pedido = null;
            //            }
            //        }
            //    }
            //}
            user.ModalEditar(id);
            return PartialView("_Editar", user);
        }

        public ActionResult Editar(Usuario usuario)
        {
            // Validar!!!!
            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                return Json("Nome inválido!");
            }

            //// Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            //// Veja mais strings de conexão em http://www.connectionstrings.com/
            //using (SqlConnection conn = new SqlConnection(""))
            //{
            //    conn.Open();

            //    // Cria um comando para inserir um novo registro à tabela
            //    using (SqlCommand cmd = new SqlCommand("UPDATE tbPedido SET Nome = @nome, Valor = @valor WHERE Id = @id", conn))
            //    {
            //        // Esses valores poderiam vir de qualquer outro lugar, como uma TextBox...
            //        cmd.Parameters.AddWithValue("@nome", pedido.Nome);
            //        cmd.Parameters.AddWithValue("@valor", pedido.Valor);
            //        cmd.Parameters.AddWithValue("@id", pedido.Id);

            //        cmd.ExecuteNonQuery();
            //    }
            //}
            user.Editar(usuario);
            return Json("ok");
        }
    }
}
