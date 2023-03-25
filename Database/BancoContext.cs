using cadastroSemaforico.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Database
{
    public class BancoContext : DbContext
    {
        public DbSet<CadastroSemaforico> CadastrosSemaforico { get; set; }
        public DbSet<DadoLogin> DadosLogin { get; set; }
<<<<<<< HEAD
        public DbSet<DbCadastroSemaforico> DbCadastroSemaforico { get; set; }
=======
>>>>>>> 531d3bd391d4dd4b83f28915752e54e0678e2dd7

        public BancoContext(DbContextOptionsBuilder<BancoContext> options) : 
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Configuração SQLite
            optionsBuilder.UseSqlite($"Filename={Constantes.CaminhoBanco}");

            //Configuração PostgreSQL
            optionsBuilder.UseNpgsql("User ID=postgres;Password=cadastro;Host=177.220.159.198;Port=5432;Database=Esteio;");
        }
    }

    public class BancoContextPostgreSQL : DbContext
    {

    }
}
