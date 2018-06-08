using Azure;
using ProjetoCRUD.BD;
using ProjetoCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoCRUD.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Modal()
        {

            return PartialView("_Modal");
        }

        public ActionResult Listar(Pessoa pessoa)
        {
            PessoaDAO pessoaDAO = new PessoaDAO();

            pessoaDAO.Adicionar(pessoa);

            using (SqlConnection conn = SqlConn.Abrir())
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT Id, Nome
                    FROM tbPessoa
                    ORDER BY Nome ASC
                    ", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Pessoa> lista = new List<Pessoa>();

                        while (reader.Read() == true)
                        {
                            lista.Add(new Pessoa()
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1)
                            });
                        }

                        return View(lista);
                    }
                }
            }
        }




        public ActionResult Upload(Pessoa pessoa)
        {

            if (Request.Files.Count == 0)
            {
                return Json("Sem arquivos!", JsonRequestBehavior.AllowGet);
            }

            if (Request.Files[0].ContentType != "image/jpeg")
            {
                return Json("Apenas imagens JPEG!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (Image bmp = Image.FromStream(Request.Files[0].InputStream))
                {
                    if (bmp.Width > 2000 || bmp.Height > 2000)
                    {
                        return Json("A imagem deve ter dimensões menores do que 2000x2000!", JsonRequestBehavior.AllowGet);
                    }
                }

                Request.Files[0].InputStream.Position = 0;
            }
            catch
            {
                return Json("Imagem JPEG inválida!", JsonRequestBehavior.AllowGet);
            }

            //Para retornar o ID do banco
            //pessoa.Id = (int)cmd.ExecuteScalar();

            AzureStorage.Upload(
                "web",
                pessoa.Id+".jpg",
                Request.Files[0].InputStream,
                "teste",
                "UseDevelopmentStorage=true");

            return Json("Sucesso!", JsonRequestBehavior.AllowGet);


        }


    }
}
