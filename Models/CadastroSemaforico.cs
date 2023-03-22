using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Models
{
    public class CadastroSemaforico
    {
        public int Id { get; set; }
        public string Rodovia { get; set; }
        public string Regional { get; set; }
        public string Sentido { get; set; }
        public string LadoPista { get; set; }
        public string AtendimentoNorma { get; set; }
        public string ObservacaoAN { get; set; }
        public string KM { get; set; }
        public string Destinacao { get; set; }
        public string TipoSinalizacao { get; set; }
        public string FormaFoco { get; set; }
        public string IndicacaoLuminosa { get; set; }
        public string SequenciaLuminosa { get; set; }
        public string EstadoConservacao { get; set; }
        public string ObservacaoEC { get; set; }
        public string Observacao { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string FotoPanoramica { get; set; }
        public string FotoDetalhe1 { get; set; }
        public string FotoDetalhe2 { get; set; }
        public string CodigoElemento { get; set; }
        public string Usuario { get; set; }
        public string IdDispositivo { get; set; }
        public string StatusInterno { get; set; }
        public int Auditoria { get; set; }
        public string DataCadastro { get; set; }
    }
}
