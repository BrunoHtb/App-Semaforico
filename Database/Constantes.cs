using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadastroSemaforico.Database
{
    public static class Constantes
    {
        public const string NomeBanco = "AppSemaforica.db3";
        public const string CaminhoBancoPostgresSQL = "User ID=postgres;Password=cadastro;Host=177.220.159.198;Port=5432;Database=Esteio;";
        public const string CaminhoDiretorioSave = "/storage/emulated/0/Android/data/com.companyname.cadastrosemaforico/cadastroSemaforico/";

        public static void CriarDiretorio()
        {
            if (!Directory.Exists(CaminhoDiretorioSave))
            {
                Directory.CreateDirectory(CaminhoDiretorioSave);
            }
        }

        public static string CaminhoBanco =>
            Path.Combine(FileSystem.AppDataDirectory, NomeBanco);
    }
}
