using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Services
{
    public class CompensationService:ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepo)
        {
            _compensationRepository = compensationRepo;
            _logger = logger;
        }

        public Compensation Create(Compensation comp)
        {
            if(comp != null)
            {
                _compensationRepository.Add(comp);
                _compensationRepository.SaveAsync().Wait();
            }

            return comp;
        }        
        
        public Compensation GetById(String id) 
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetById(id);
            }

            return null;
        }

    }
}
