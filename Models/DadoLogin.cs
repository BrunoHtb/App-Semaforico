using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Models
{
    public class DadoLogin
    {
        public  int Id { get; set; }
        public int IdDispositivo { get; set; }
        public string NomeUsuario { get; set; }
        public int Auditoria { get; set; }
    }
}
