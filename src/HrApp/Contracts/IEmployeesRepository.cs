using HrApp.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IEmployeesRepository
    {
        Task<List<EmployeeEntity>> GetEmployees(Division division);
        Task<List<EmployeeEntity>> GetEmployeesWhoAreNotInBussinessTrip(Division division, DateTime lunchDate);
        Task UpdateEmployeeTimeWorked(string employeeId, double time);
        Task<EmployeeEntity> GetEmployeeByLastName(string lastname);
        Task<EmployeeEntity> GetEmployeeById(string employeeId);

        Task<List<EmployeeEntity>> GetEmployeesWhoNotAbsence(List<EmployeeEntity> employees, DateTime lunchDate);

    }
}