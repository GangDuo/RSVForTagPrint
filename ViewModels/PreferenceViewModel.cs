using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using RSVForTagPrint.Helpers;
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

        public PreferenceViewModel()
        {
            this.Preference = new Models.Preference()
            {
                HostName = Properties.Settings.Default.FmwwHostName,
                Group = Properties.Settings.Default.FmwwAccessKeyId,
                User = Properties.Settings.Default.FmwwUserName,
                ApiServer = Properties.Settings.Default.VirgoApiUri,
                GroupPassword = Password.Read(Preferences.CNGroupPassword),
                UserPassword = Password.Read(Preferences.CNUserPassword),
            };

            this.CancelCommand = new DelegateCommand(() =>
            {
                Preference.HostName = Properties.Settings.Default.FmwwHostName;
                Preference.Group = Properties.Settings.Default.FmwwAccessKeyId;
                Preference.User = Properties.Settings.Default.FmwwUserName;
                Preference.ApiServer = Properties.Settings.Default.VirgoApiUri;

                Preference.GroupPassword = Password.Read(Preferences.CNGroupPassword);
                Preference.UserPassword = Password.Read(Preferences.CNUserPassword);
                this.FinishInteraction();
            });

            this.OKCommand = new DelegateCommand(() =>
                {
                    Properties.Settings.Default.FmwwHostName = Preference.HostName;
                    Properties.Settings.Default.FmwwAccessKeyId = Preference.Group;
                    Properties.Settings.Default.FmwwUserName = Preference.User;
                    Properties.Settings.Default.VirgoApiUri = Preference.ApiServer;
                    Properties.Settings.Default.Save();

                    Password.Save(Preferences.CNGroupPassword, this.Preference.GroupPassword);
                    Password.Save(Preferences.CNUserPassword, this.Preference.UserPassword);

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