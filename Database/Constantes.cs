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

        public static string CaminhoBanco =>
            Path.Combine(FileSystem.AppDataDirectory, NomeBanco);
    }
}
