using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    //Compensation Class containing employee information, the salary and effective date
    public class Compensation
    {
        public String CompensationId { get; set; }

        //Created an enum for better representation of dates
        enum Date { Month, Day, Year };

        public Employee employee { get; set; }

        public int salary { get; set; }

        public int[] effectiveDate = new int[3];
        

    }
}
