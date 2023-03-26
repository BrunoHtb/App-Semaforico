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

        public BancoContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Constantes.CaminhoBanco}");
        }
    }
}
