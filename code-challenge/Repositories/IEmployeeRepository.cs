﻿using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        ReportingStructure GetReportingStructureById(Employee employee);
        //void resetNum();
        int GetNumReportsById(String id);
        Task SaveAsync();


    }
}