using cadastroSemaforico.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Database
{
    public class BancoContextSQLite : DbContext
    {
        public DbSet<CadastroSemaforico> CadastrosSemaforico { get; set; }
        public DbSet<DadoLogin> DadosLogin { get; set; }

        public BancoContextSQLite()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Configuração SQLite
            optionsBuilder.UseSqlite($"Filename={Constantes.CaminhoBanco}");
        }
    }

    public class BancoContextPostgreSQL : DbContext
    {
        public DbSet<SemaforicoPostgreSQLDB> tb_sistemasemaforico { get; set; }

        public BancoContextPostgreSQL()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Configuração PostgreSQL
            optionsBuilder.UseNpgsql(Constantes.CaminhoBancoPostgresSQL);
        }
    }
}
