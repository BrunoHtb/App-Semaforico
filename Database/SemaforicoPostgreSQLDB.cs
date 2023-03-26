using cadastroSemaforico.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Database
{
    public class CadastroPostgresSQLDB
    {
        private BancoContextPostgreSQL Banco { get; set; }

        public CadastroPostgresSQLDB()
        {
            Banco = new BancoContextPostgreSQL();

        }//Retorna a lista toda
        public async Task<List<SemaforicoPostgreSQLDB>> PesquisarAsync()
        {
            return await Banco.tb_ss.ToListAsync();
        }

        //Cadastra elemento novo
        public async Task<bool> CadastrarAsync(List<SemaforicoPostgreSQLDB> listaCadastroSemaforico)
        {
            try
            {
                Banco.tb_ss.AddRange(listaCadastroSemaforico);
                int linhas = Banco.SaveChanges();
                return (linhas > 0) ? true : false;
            }
            catch (DbUpdateException ex)
            {
                // Exibir a mensagem de erro da exceção interna
                Console.WriteLine(ex.InnerException.Message);
                return false;
            }
        }

        //Edita elemento novo
        public async Task<bool> AtualizarAsync(SemaforicoPostgreSQLDB cadastroSemaforico)
        {
            Banco.tb_ss.Update(cadastroSemaforico);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }


    }
}