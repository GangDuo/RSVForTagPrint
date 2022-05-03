using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RSVForTagPrint.Models
{
    [DataContract]
    class Tag
    {
        [DataMember(Name = "id")]
        public ulong Id { get; set; }

        [DataMember(Name = "owner")]
        public string Owner { get; set; }
        
        [DataMember(Name = "jan")]
        public string Jan { get; set; }
        
        [DataMember(Name = "visible")]
        public bool Visible { get; set; }
    }
}
