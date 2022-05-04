using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSVForTagPrint.Models
{
    public class Preference : BindableBase
    {
        private string _Group;
        public string Group
        {
            get { return this._Group; }
            set { this.SetProperty(ref this._Group, value); }
        }

        private string _GroupPassword;
        public string GroupPassword
        {
            get { return this._GroupPassword; }
            set { this.SetProperty(ref this._GroupPassword, value); }
        }

        private string _User;
        public string User
        {
            get { return this._User; }
            set { this.SetProperty(ref this._User, value); }
        }

        private string _UserPassword;
        public string UserPassword
        {
            get { return this._UserPassword; }
            set { this.SetProperty(ref this._UserPassword, value); }
        }

        private string _HostName;
        public string HostName
        {
            get { return this._HostName; }
            set { this.SetProperty(ref this._HostName, value); }
        }

        private string _ApiServer;
        public string ApiServer
        {
            get { return this._ApiServer; }
            set { this.SetProperty(ref this._ApiServer, value); }
        }
    }
}
