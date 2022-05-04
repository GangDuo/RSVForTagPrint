using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace RSVForTagPrint.Helpers
{
    class Password
    {
        private static string GetFilePathBy(string containerName)
        {
            var fullAssemblyNmae = Assembly.GetExecutingAssembly().Location;
            var d = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Path.GetFileNameWithoutExtension(fullAssemblyNmae));
            Directory.CreateDirectory(d);
            return Path.Combine(d, containerName + ".dat");
        }

        public static void Save(string containerName, string password)
        {
            var fileName = GetFilePathBy(containerName);
            try
            {
                // Create a key and save it in a container.
                var pubKey = Helpers.Persistence.Container.CreatePubKeyIn(containerName);

                //書き込むファイルを開く（UTF-8 BOM無し）
                using (var stream = new StreamWriter(fileName, false, new System.Text.UTF8Encoding(false)))
                {
                    var data = Helpers.Persistence.Encrypt(password, pubKey);
                    stream.Write(data);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string Read(string containerName)
        {
            var fileName = GetFilePathBy(containerName);
            try
            {
                //読み込むファイルを開く
                using (var sr = new StreamReader(fileName, new System.Text.UTF8Encoding(false)))
                {
                    return Helpers.Persistence.Container.Decrypt(sr.ReadToEnd(), containerName);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FileNotFoundException e)
            {
                return String.Empty;
            }
            throw new Exception();
        }
    }
}
