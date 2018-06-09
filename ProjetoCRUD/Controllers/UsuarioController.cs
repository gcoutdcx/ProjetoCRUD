using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace ProjetoCRUD
{
    //CREATE TABLE tbPedido (
    //    Id INT NOT NULL IDENTITY PRIMARY KEY,
    //    Nome VARCHAR(50) NOT NULL,
    //    Valor FLOAT NOT NULL
    //)

    public class UsuarioController : Controller
    {
        // GET: Pedido
        public ActionResult Index()
        {
            List<Pedido> lista = new List<Pedido>();

            // Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            // Veja mais strings de conexão em http://www.connectionstrings.com/
            using (SqlConnection conn = new SqlConnection(""))
            {
                conn.Open();

                // Cria um comando para selecionar registros da tabela, trazendo todas as pessoas que nasceram depois de 1/1/1900
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Nome, Valor FROM tbPedido ORDER BY Nome ASC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Obtém os registros, um por vez
                        while (reader.Read() == true)
                        {
                            int id = reader.GetInt32(0);
                            string nome = reader.GetString(1);
                            double valor = reader.GetDouble(2);

                            Pedido pedido = new Pedido();
                            pedido.Id = id;
                            pedido.Nome = nome;
                            pedido.Valor = valor;

                            lista.Add(pedido);
                        }
                    }
                }
            }

            return View(lista);
        }

        public ActionResult Criar(Pedido pedido)
        {
            // Validar!!!!
            if (string.IsNullOrWhiteSpace(pedido.Nome))
            {
                return Json("Nome inválido!");
            }

            // Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            // Veja mais strings de conexão em http://www.connectionstrings.com/
            using (SqlConnection conn = new SqlConnection(""))
            {
                conn.Open();

                // Cria um comando para inserir um novo registro à tabela
                using (SqlCommand cmd = new SqlCommand("INSERT INTO tbPedido (Nome, Valor) OUTPUT INSERTED.Id VALUES (@nome, @valor)", conn))
                {
                    // Esses valores poderiam vir de qualquer outro lugar, como uma TextBox...
                    cmd.Parameters.AddWithValue("@nome", pedido.Nome);
                    cmd.Parameters.AddWithValue("@valor", pedido.Valor);

                    int id = (int)cmd.ExecuteScalar();
                }
            }

            return Json("ok");
        }

        public ActionResult ModalEditar(int id)
        {
            Pedido pedido;

            // Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            // Veja mais strings de conexão em http://www.connectionstrings.com/
            using (SqlConnection conn = new SqlConnection(""))
            {
                conn.Open();

                // Cria um comando para selecionar registros da tabela, trazendo todas as pessoas que nasceram depois de 1/1/1900
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Nome, Valor FROM tbPedido WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Obtém os registros, um por vez
                        if (reader.Read() == true)
                        {
                            string nome = reader.GetString(1);
                            double valor = reader.GetDouble(2);

                            pedido = new Pedido();
                            pedido.Id = id;
                            pedido.Nome = nome;
                            pedido.Valor = valor;
                        }
                        else
                        {
                            pedido = null;
                        }
                    }
                }
            }

            return PartialView("_Editar", pedido);
        }

        public ActionResult Editar(Pedido pedido)
        {
            // Validar!!!!
            if (string.IsNullOrWhiteSpace(pedido.Nome))
            {
                return Json("Nome inválido!");
            }

            // Cria e abre a conexão com o banco de dados (essa string só serve para acessar o banco localmente)
            // Veja mais strings de conexão em http://www.connectionstrings.com/
            using (SqlConnection conn = new SqlConnection(""))
            {
                conn.Open();

                // Cria um comando para inserir um novo registro à tabela
                using (SqlCommand cmd = new SqlCommand("UPDATE tbPedido SET Nome = @nome, Valor = @valor WHERE Id = @id", conn))
                {
                    // Esses valores poderiam vir de qualquer outro lugar, como uma TextBox...
                    cmd.Parameters.AddWithValue("@nome", pedido.Nome);
                    cmd.Parameters.AddWithValue("@valor", pedido.Valor);
                    cmd.Parameters.AddWithValue("@id", pedido.Id);

                    cmd.ExecuteNonQuery();
                }
            }

            return Json("ok");
        }
    }
}
