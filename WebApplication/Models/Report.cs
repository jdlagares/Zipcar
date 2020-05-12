using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Enumerations;

namespace WebApplication.Models
{
    public class Report
    {
        public int Id { get; set; }

        public bool Resolved { get; set; }

        public EnumReportType Type { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}
