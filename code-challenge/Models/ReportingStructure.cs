using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    //Class that contains Employee information, and the number of reports and employee has assigned to them.
    public class ReportingStructure
    {
        public Employee employee { get; set; }

        public int numberOfReports { get; set; } 

    }
}
