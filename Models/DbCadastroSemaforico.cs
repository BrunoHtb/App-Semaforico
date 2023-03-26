using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Models
{
    public class SemaforicoPostgreSQLDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ss_id { get; set; }
        public string regional_der { get; set; }
        public string rodovia { get; set; }
        public string codigo { get; set; }
        public string data_levantamento { get; set; }
        public string km { get; set; }
        public string lado_pista { get; set; }
        public string sentido { get; set; }
        public string tipo_sinalização { get; set; }
        public string destinacao { get; set; }
        public string numero_indicacoes_luminosas { get; set; }
        public string sequencia_luminosa { get; set; }
        public string forma_foco { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string nome_fotopanoramica { get; set; }
        public string nome_fotodetalhe1 { get; set; }
        public string nome_fotodetalhe2 { get; set; }
        public string condicao { get; set; }
        public string observacao_ec { get; set; }
        public string atendimento_norma { get; set; }
        public string obs_an { get; set; }
        public string observacao { get; set; }
        public string usuariologado { get; set; }
        public string status_interno { get; set; }
        public string empresa { get; set; }
        public string alteracao_dia { get; set; }
        public string observacao_sistema { get; set; }
        public string data_inclusao_novo { get; set; }
        public string pista { get; set; }
        public int auditoria { get; set; }
        public string numero_dispositivo { get; set; }
        public string usuario_campo { get; set; }
    }
}
