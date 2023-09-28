﻿using System;
using CodeChallenge.Models;

namespace CodeChallenge.Services
{
	public interface IReportingStructureService
	{
		ReportingStructure GetByEmployeeId(string employee_id);
	}
}

