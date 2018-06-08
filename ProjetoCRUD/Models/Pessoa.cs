using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoCRUD.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public DateTime Nascimento { get; set; }
        public double Peso { get; set; }

        
    }
}