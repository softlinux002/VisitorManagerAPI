using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class State
    {
        public int Id { get; set; }
        public int zone_id { get; set; }
        public int country_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string status { get; set; }
    }
}