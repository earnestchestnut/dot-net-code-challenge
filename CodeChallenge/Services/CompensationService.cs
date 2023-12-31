﻿using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
	public class CompensationService : ICompensationService
	{
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

		public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
		{
			_compensationRepository = compensationRepository;
			_logger = logger;
		}

		public Compensation Create(Compensation compensation)
		{
			if(compensation is not null)
			{
				_compensationRepository.Add(compensation);
				_compensationRepository.SaveAsync().Wait();
            }

			return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            if (!string.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetByEmployeeId(employeeId);
            }

            return null;
        }

    }
}

