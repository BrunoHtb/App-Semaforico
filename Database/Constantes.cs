//using Android.OS.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using FileSystem = Microsoft.Maui.Storage.FileSystem;

namespace cadastroSemaforico.Database
{
    public static class Constantes
    {
        public const string NomeBanco = "AppSemaforica.db3";
        public const string CaminhoBancoPostgresSQL = "User ID=postgres;Password=cadastro;Host=177.220.159.198;Port=5432;Database=Esteio;";
        public const string CaminhoDiretorioSave = "/storage/emulated/0/Download/cadastroSemaforico/";
         
        public static string CaminhoBanco =>
            Path.Combine(FileSystem.AppDataDirectory, NomeBanco);
    }
}
