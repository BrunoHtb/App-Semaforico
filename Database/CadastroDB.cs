using cadastroSemaforico.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Database
{
    public class CadastroDB
    {
        private BancoContext Banco { get; set; }

        public CadastroDB()
        {
            Banco = new BancoContext();
        }

        //Retorna a lista toda
        public async Task<List<CadastroSemaforico>> PesquisarAsync()
        {
            return await Banco.CadastrosSemaforico.ToListAsync();
        }

        //Cadastra elemento novo
        public async Task<bool> CadastrarAsync(CadastroSemaforico cadastroSemaforico)
        {
            Banco.CadastrosSemaforico.Add(cadastroSemaforico);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        //Edita elemento novo
        public async Task<bool> AtualizarAsync(CadastroSemaforico cadastroSemaforico)
        {
            Banco.CadastrosSemaforico.Update(cadastroSemaforico);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        //Deleta elemento novo
        public async Task<bool> ExcluirAsync(int id)
        {
            CadastroSemaforico cadastro = await ConsultarAsync(id);
            Banco.CadastrosSemaforico.Remove(cadastro);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }
        public async Task<CadastroSemaforico> ConsultarAsync(int id)
        {
            return await Banco.CadastrosSemaforico.FindAsync(id);
        }


        /* --------------------------------------------------------------------------------------------------------- */
        /*                                              LOGIN                                                        */
        /* --------------------------------------------------------------------------------------------------------- */
        //Cadastra elemento novo LOGIN
        public async Task<bool> CadastrarLoginAsync(DadoLogin dadosLogin)
        {
            Banco.DadosLogin.Add(dadosLogin);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        //Edita elemento novo LOGIN
        public async Task<bool> AtualizarLoginAsync(DadoLogin dadosLogin)
        {
            Banco.DadosLogin.Update(dadosLogin);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        //Retorna a lista toda
        public async Task<List<DadoLogin>> PesquisarLoginAsync()
        {
            return await Banco.DadosLogin.ToListAsync();
        }

    }
}
