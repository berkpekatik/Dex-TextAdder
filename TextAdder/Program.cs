using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdder
{
    class Program
    {
        private static List<string> paths = new List<string>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1-) Dosyalara ekle");
                Console.WriteLine("2-) Dosyalardan kaldır");
                var answer = Console.ReadLine();
                if (answer == "1") { Write(); break; }
                if (answer == "2") { Delete(); break; }
            }
        }
        private static void Delete()
        {
            Console.Write("Resource konumunu girin: ");
            var filesPath = Console.ReadLine();
            paths.AddRange(Directory.GetFiles(@filesPath, "__resource.lua", SearchOption.AllDirectories));
            paths.AddRange(Directory.GetFiles(@filesPath, "fxmanifest.lua", SearchOption.AllDirectories));
            foreach (var path in paths)
            {
                if (DeleteArgs(path))
                {
                    Console.WriteLine("Başarıyla Değiştirildi " + path);
                }
                else
                {
                    Console.WriteLine("Bir hata oluştu! " + path);
                }
            }
            Console.ReadKey();
        }
        private static void Write()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\dexelirac.lua"))
            {
                Console.Write("Program klasöründe dexelirac.lua bulunamadı!");
                Console.ReadKey();
                return;
            }
            Console.Write("Resource konumunu girin: ");
            var filesPath = Console.ReadLine();
            var resourceList = Directory.GetFiles(@filesPath, "__resource.lua", SearchOption.AllDirectories);
            var manifestList = Directory.GetFiles(@filesPath, "fxmanifest.lua", SearchOption.AllDirectories);
            paths.AddRange(resourceList);
            paths.AddRange(manifestList);
            foreach (var path in paths)
            {
                if (!ControlText(path, @"client_script 'dexelirac.lua'"))
                {
                    Console.WriteLine("Zaten kayıtlı! " + path);
                }
                else
                {
                    if (!AddText(path, @"client_script 'dexelirac.lua'"))
                    {
                        Console.WriteLine("Bir hata oluştu! " + path);
                    }
                    else
                    {
                        Console.WriteLine("Başarıyla eklendi! " + path);
                        if (!PasteSc(path))
                        {
                            Console.WriteLine("dexelirac.lua " + path + " klasörüne eklenemedi!");
                        }

                    }
                }
            }
            Console.ReadKey();
        }
        private static bool ControlText(string path, string text)
        {
            var file = File.ReadAllLines(path);
            if (file.Where(x => x == text).Any())
            {
                return false;
            }
            return true;
        }
        private static bool AddText(string path, string text)
        {
            try
            {
                var file = File.ReadAllText(path);
                File.WriteAllText(path, file + "\n" + text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool DeleteArgs(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var file = File.ReadAllLines(path).ToList();
                    foreach (var item in file)
                    {
                        if (item == "client_script 'dexelirac.lua'")
                        {
                            file.Remove(item);
                            break;
                        }
                    }
                    var newString = string.Join(Environment.NewLine, file);
                    File.WriteAllText(path, newString);
                }
                var newPath = path.Replace("__resource.lua", "");
                newPath = newPath.Replace("fxmanifest.lua", "");
                if (File.Exists(newPath + @"\dexelirac.lua"))
                {
                    File.Delete(newPath + @"\dexelirac.lua");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool PasteSc(string path)
        {
            try
            {
                var newPath = path.Replace("__resource.lua", "");
                newPath = newPath.Replace("fxmanifest.lua", "");
                File.Copy(Environment.CurrentDirectory + @"\dexelirac.lua", newPath + @"\dexelirac.lua");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
