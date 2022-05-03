using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSVForTagPrint.Models
{
    sealed class Environment
    {
        private static readonly Encoding SjisEnc = Encoding.GetEncoding("Shift_JIS");
        private static readonly string HistoryFile = @".histories";
        public ObservableCollection<Models.Job> Histories { get; private set; }

        private static Environment _singleInstance = new Environment();

        public static Environment Instance { get { return _singleInstance; } }

        public void Save()
        {
            using (var writer = new StreamWriter(HistoryFile, false, SjisEnc))
            {
                var latestJobs = this.Histories.Reverse().Take(10).Reverse();
                writer.WriteLine(Text.Json.ToString<IEnumerable<Models.Job>>(latestJobs));
            }
        }

        private Environment()
        {
            try
            {
                Initialize();
            }
            catch (FileNotFoundException)
            {
                File.Create(HistoryFile).Close();
                Initialize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Initialize()
        {
            using (var sr = new StreamReader(HistoryFile, SjisEnc))
            {
                var text = sr.ReadToEnd();
                if (text.Length == 0)
                {
                    this.Histories = new ObservableCollection<Models.Job>();
                }
                else
                {
                    this.Histories = Text.Json.Parse<ObservableCollection<Models.Job>>(text);
                }
            }
        }
    }
}
