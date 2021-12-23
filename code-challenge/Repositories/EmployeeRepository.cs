using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        //private static int numOfReports = 0;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        //Method to create the Reporting Structure object that will be received by the service.
        public ReportingStructure GetReportingStructureById(Employee employee)
        {
            //Creates the ReportingStructure object with the employee information required.
            ReportingStructure reportingStructure = new ReportingStructure
            {
                employee = _employeeContext.Employees
                           .Include(i => i.DirectReports)
                           .FirstOrDefault(i => i.EmployeeId == employee.EmployeeId),

                numberOfReports = GetNumReportsById(employee.EmployeeId)
            };
            
            return reportingStructure;
        }

        //Method to compute the number of reports for an employee, by id
        public int GetNumReportsById(String id)
        {
            int total = 0;
            Queue<Employee> EQueue = new Queue<Employee>(_employeeContext.Employees
                                  .Include(i => i.DirectReports)
                                  .FirstOrDefault(i => i.EmployeeId == id).DirectReports);
            
            //While there exists Direct Reports for this employee
            while(EQueue.Count > 0) 
            {
                Employee emp = EQueue.Dequeue();
                total += 1;

                //Creates an IEnum directReports that contains the directReports of the current employee. 
                //This is traversable with a for each statement
                IEnumerable<Employee> directReports = _employeeContext.Employees.Include(i => i.DirectReports)
                    .FirstOrDefault(i => i.EmployeeId == emp.EmployeeId).DirectReports;


                //For each Direct report in Direct Reports, add to my EQueue
                foreach(var dirRep in directReports)
                EQueue.Enqueue(dirRep);
            }
            return total;
        }

        ///Initial attempt of using Recursion to compute the number of Direct reports for employee
        //int numDirects;

        //if(employee.DirectReports == null) 
        //{
        //    numDirects = 0;
        //}
        //else
        //numDirects = employee.DirectReports.Count;

        //if(numDirects!= 0)
        //{
        //    foreach(Employee E in employee.DirectReports) 
        //    {
        //        numOfReports = countReports(E);
        //    }
        //}
        //return numDirects + numOfReports;

        //Resets the number
        //public void resetNum()
        //{
        //    numOfReports = 0;
        //} 
    }
}
