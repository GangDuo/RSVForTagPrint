using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RSVForTagPrint.Models
{
    [DataContract]
    class Job
    {
        private static readonly string DateTimeFormat = "ddd MMM dd HH:mm:ss zzz yyyy";

        [DataMember(Name = "codeToPrint")]
        public string CodeToPrint { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "createdAt")]
        private string created_at_str_prop
        {
            get
            {
                return created_at_str_field;
            }

            set
            {
                created_at_field = DateTime.ParseExact(value, DateTimeFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
                created_at_str_field = value;
            }
        }
        private string created_at_str_field;

        private DateTime created_at_field;
        public DateTime CreatedAt
        {
            get
            {
                return created_at_field;
            }

            set
            {
                created_at_str_field = value.ToString(DateTimeFormat, DateTimeFormatInfo.InvariantInfo);
                created_at_field = value;
            }
        }
    }
}
