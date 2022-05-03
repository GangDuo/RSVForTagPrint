using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RSVForTagPrint.ViewModels
{
    class PreferenceViewModel : IInteractionRequestAware
    {
        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        public Models.Preference Preference { get; set; }

        public DelegateCommand OKCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        private static string CNGroupPassword = "GroupPassword";
        private static string CNUserPassword = "UserPassword";

        private class Password
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
                throw new Exception();
            }
        }

        public PreferenceViewModel()
        {
            this.Preference = new Models.Preference();

            this.CancelCommand = new DelegateCommand(() =>
            {
                Debug.WriteLine("CancelCommand");
                Preference.GroupPassword = Password.Read(CNGroupPassword);
                this.FinishInteraction();
            });

            this.OKCommand = new DelegateCommand(() =>
                {
                    Password.Save(CNGroupPassword, this.Preference.GroupPassword);
                    Helpers.Persistence.Container.DeleteKeys(CNGroupPassword);

                    this.FinishInteraction();
                });
                //, () => !string.IsNullOrWhiteSpace(this.Preference.Group)
                //        && !string.IsNullOrWhiteSpace(this.Preference.GroupPassword)
                //        && !string.IsNullOrWhiteSpace(this.Preference.User)
                //        && !string.IsNullOrWhiteSpace(this.Preference.UserPassword))
                //.ObservesProperty(() => this.Preference.Group);
        }

    }
}
/**
 * https://github.com/runceel/PrismEdu/tree/master/05.InteractionRequest
 * http://someprog.blog.fc2.com/blog-entry-12.html
 * http://blog.hiros-dot.net/?page_id=4083
 * http://dobon.net/vb/dotnet/string/rsaencryption.html
*/