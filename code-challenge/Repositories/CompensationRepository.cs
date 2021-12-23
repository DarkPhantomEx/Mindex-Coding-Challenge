using System;
using System.Collections.Generic;
using System.Linq;
using challenge.Models;
using challenge.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRepository:ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger; 
                
        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext )
        {
            _logger = logger;
            _employeeContext = employeeContext;
        }

        //Returns the Compensation that corresponds to the id specified
        public Compensation GetById(string id) 
        {
            return _employeeContext.Compensations
                .Include(c => c.employee)
                .SingleOrDefault(c => c.employee.EmployeeId == id);
        }

        //Adds a Compensation that is specified into the database
        public Compensation Add(Compensation comp)
        {
            comp.employee = _employeeContext
                .Employees
                .FirstOrDefault(c => c.EmployeeId == comp.employee.EmployeeId);

            _employeeContext.Compensations.Add(comp);

            return comp;
        }

        public Task SaveAsync() 
        {
            return _employeeContext.SaveChangesAsync();
        }

    }
}
