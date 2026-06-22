using System.IO;
using Microsoft.Win32;
namespace RestaurantCarol.Helpers
{
    public static class ImageUploadHelper
    {
        private const string RUNTIME_FOLDER = "Images/runtime";
        private static readonly string[] EXTENSII_ACCEPTATE = { ".jpg", ".jpeg", ".png" };
        private const long MAX_FILE_SIZE = 5 * 1024 * 1024;
        public static string? AlegePoza()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Alege o poza",
                Filter = "Imagini (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        public static string CopiazaPozaInRuntime(string caleSursa, string prefixNume)
        {
            if (!File.Exists(caleSursa))
                throw new Exception("Fisierul selectat nu exista.");
            string extensie = Path.GetExtension(caleSursa).ToLower();
            if (Array.IndexOf(EXTENSII_ACCEPTATE, extensie) == -1)
                throw new Exception($"Extensia '{extensie}' nu este acceptata. Folosesti JPG sau PNG.");
            FileInfo info = new FileInfo(caleSursa);
            if (info.Length > MAX_FILE_SIZE)
                throw new Exception($"Fisierul e prea mare (max {MAX_FILE_SIZE / 1024 / 1024} MB).");
            string folderRuntime = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                RUNTIME_FOLDER);
            if (!Directory.Exists(folderRuntime))
                Directory.CreateDirectory(folderRuntime);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string numeUnic = $"{prefixNume}_{timestamp}{extensie}";
            string caleDestinatie = Path.Combine(folderRuntime, numeUnic);
            File.Copy(caleSursa, caleDestinatie, overwrite: false);
            return $"{RUNTIME_FOLDER}/{numeUnic}";
        }

        public static string ConstruiestePathPentruImage(string calePoza)
        {
            if (calePoza.Contains("runtime/", StringComparison.OrdinalIgnoreCase) ||
                calePoza.Contains("runtime\\", StringComparison.OrdinalIgnoreCase))
            {
                string caleAbsoluta = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    calePoza.Replace("/", "\\"));
                if (File.Exists(caleAbsoluta))
                    return caleAbsoluta;
            }
            string caleNormalizata = calePoza.StartsWith("/") ? calePoza : "/" + calePoza;
            return $"pack://application:,,,{caleNormalizata}";
        }
    }
}