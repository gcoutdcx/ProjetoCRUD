using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using ProjetoCRUD.Models;
using System.Web;
using System.Linq;
using System.Data;
using System.Drawing;
using Azure;

namespace ProjetoCRUD
{
    public class UsuarioController : Controller
    {
  
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

            using (SqlConnection conn = Conexao.Abrir())
            {
                // Cria um comando para inserir um novo registro à tabela
                using (SqlCommand cmd = new SqlCommand("INSERT INTO USUARIO (Nome, Email) OUTPUT INSERTED.Id VALUES (@nome, @email)", conn))
                {
                    // Esses valores poderiam vir de qualquer outro lugar, como uma TextBox...
                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);

                    int id = (int)cmd.ExecuteScalar();
                    Upload(usuario, id);
                }
            }

            


            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModalEditar(int id)
        {
            Usuario usuario;

            using (SqlConnection conn = Conexao.Abrir())
            {
                // Cria um comando para selecionar registros da tabela, trazendo todas as pessoas que nasceram depois de 1/1/1900
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
            return PartialView("_Editar", usuario);
        }

        public ActionResult Editar(Usuario usuario)
        {
            // Validar!!!!
            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                return Json("Nome inválido!");
            }

            using (SqlConnection conn = Conexao.Abrir())
            {

                // Cria um comando para inserir um novo registro à tabela
                using (SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET Nome = @nome, Email = @email WHERE Id = @id", conn))
                {
                    // Esses valores poderiam vir de qualquer outro lugar, como uma TextBox...
                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.Parameters.AddWithValue("@id", usuario.Id);

                    Upload(usuario, usuario.Id);
                    cmd.ExecuteNonQuery();
                    
                }
            }

            return Json("ok");
        }

        public ActionResult Excluir(int id)
        {
            using (SqlConnection conn = Conexao.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@" DELETE FROM USUARIO
                                                         WHERE ID = @id", conn))

                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Upload(Usuario usuario, int idUsuario)
        {
            if (Request.Files.Count == 0)
            {
                return Json("Sem arquivos!", JsonRequestBehavior.AllowGet);
            }

            if (Request.Files[0].ContentType != "image/jpeg")
            {
                return Json("Apenas Imagens JPEG!", JsonRequestBehavior.AllowGet);
            }


            try
            {
                using (Image bmp = Image.FromStream(Request.Files[0].InputStream))
                {
                    if (bmp.Width > 2000 || bmp.Height > 2000)
                    {
                        return Json("A imagem deve ter dimensões menores do que o padrão!", JsonRequestBehavior.AllowGet);
                    }

                }

                Request.Files[0].InputStream.Position = 0;
            }
            catch
            {
                return Json("Imagem JPEG Inválida!", JsonRequestBehavior.AllowGet);
            }

            AzureStorage.Upload(
                "web",
                idUsuario+".jpg",
                Request.Files[0].InputStream,
                "teste",
                "UseDevelopmentStorage=true");

            return Json("Sucesso!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModalCadastro()
        {
            return PartialView("_Cadastro");
        }

        public ActionResult ModalExcluir(Usuario user)
        {
            return PartialView("_Excluir", user);
        }

       
    }
}
