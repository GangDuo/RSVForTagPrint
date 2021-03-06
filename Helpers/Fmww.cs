using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M = FMWW.Master;

namespace RSVForTagPrint.Helpers
{
    class Fmww
    {
        public static void Upload(string fullName)
        {
            FMWW.Core.Config.Instance.HostName = Preferences.AsString(Preferences.HostName);
            var uploader = new M.PriceTag.New.Page()
            {
                UserAccount = new FMWW.Entity.UserAccount()
                {
                    UserName = Preferences.AsString(Preferences.AccessKeyId),
                    Person = Preferences.AsString(Preferences.UserName),
                    Password = Preferences.AsString(Preferences.CNGroupPassword),
                    PersonPassword = Preferences.AsString(Preferences.CNUserPassword)
                },
                PathShiftJis = fullName
            };
            if (uploader.CanExecute(null))
            {
                uploader.Execute(null);
                Console.WriteLine(String.Format("source = {0}\n{1}\n------------------------------------",
                    fullName, uploader.ResultMessage));
            }
        }

        public static void WriteFile(string fullName, string id, IEnumerable<string> janCodes)
        {
            var headers = new string[]
            {
                "コード,名称,,", 
                String.Format("{0},{1},,", id, "Owner is system."),
                "バーコード,枚数,品名上段,品名下段"
            };
            var writer = new StreamWriter(fullName, false, Encoding.GetEncoding("Shift_JIS"));
            foreach (var line in headers)
            {
                writer.WriteLine(line);
            }
            foreach (var jan in janCodes)
            {
                writer.WriteLine(String.Format("{0},1,,", jan));
            }
            writer.Close();
        }
    }
}
