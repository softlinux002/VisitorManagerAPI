using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class Country
    {
        public int Id { get; set; }
        public int country_id { get; set; }
        public string name { get; set; }
        public string iso_code_2 { get; set; }
        public string iso_code_3 { get; set; }
        public string address_format { get; set; }
        public string postcode_required { get; set; }
        public string status { get; set; }
    }
}