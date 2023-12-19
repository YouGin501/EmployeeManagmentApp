using BLL.Contracts;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository repository)
        {
            _employeeRepository = repository;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var result = await _employeeRepository.GetAll();
            return result;
        }
        public async Task<Employee> GetById(int id)
        {
            var result = await _employeeRepository.GetById(id);
            return result;
        }

        public async Task AddEmployee(Employee employee)
        {
            await _employeeRepository.AddEmployee(employee);
        }
        public async Task UpdateEmployee(int id, Employee employee)
        {
            await _employeeRepository.UpdateEmployee(id, employee);
        }
        public async Task DeleteEmployee(int id)
        {
            await _employeeRepository.DeleteEmployee(id);
        }

        public async Task<IEnumerable<Employee>> GeneralSalaryInfo(DateTime startPeriod, DateTime endPeriod, int? departmentId, int? positionId)
        {
            var result = await _employeeRepository.GeneralSalaryInfo(startPeriod, endPeriod, departmentId, positionId);
            return result;
        }
    }
}
