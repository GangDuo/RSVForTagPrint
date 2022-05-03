using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RSVForTagPrint.Helpers
{
    class VirgoApi
    {
        private static readonly string HomeUrl = Environment.GetEnvironmentVariable("VIRGO_API_URI");
        public List<string> UniqueJanCodes { get; private set; }

        private List<Models.Tag> RawSource { get; set; }

        public void FetchSource()
        {
            var url = String.Format(@"{0}tags.json?", HomeUrl);
            var q = String.Join("&", new Dictionary<string, string>()
                    {
                        {"q[owner_eq]", ""},
                        {"q[jan_cont]", ""},
                        {"q[visible_true]", "true"}
                    }.Select(pair =>
                    {
                        return String.Join("=",
                            new string[] { pair.Key, pair.Value }
                            .Select(v => System.Web.HttpUtility.UrlEncode(v)));
                    }));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var req = System.Net.WebRequest.Create(url + q);
            //req.Headers.Add("Accept-Language:ja,en-us;q=0.7,en;q=0.3");
            WebResponse res = default(WebResponse);
            res = req.GetResponse(); // res = System.Net.WebResponse
            var resStream = res.GetResponseStream(); // resStream = System.IO.Streamクラス
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Models.Tag>));
            this.RawSource = (List<Models.Tag>)serializer.ReadObject(resStream);
            this.UniqueJanCodes = RawSource.Select(a => a.Jan).Distinct().ToList();
            resStream.Close();
            res.Close();
        }

        public void DestroyAll() 
        {
            foreach (var r in RawSource)
            {
                string sURL = String.Format(@"{0}tags/{1}.json", HomeUrl, r.Id);
                var request = System.Net.WebRequest.Create(sURL);
                request.Method = "DELETE";
                var response = (System.Net.HttpWebResponse)request.GetResponse();
                request.Abort();
            }
        }
    }
}
