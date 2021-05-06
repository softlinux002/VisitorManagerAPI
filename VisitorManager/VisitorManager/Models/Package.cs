using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorManager.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public double NoOfVisitorSupported { get; set; }
        public double HalfYearlyPrice { get; set; }
        public double AnnuallyPrice { get; set; }
    }
}