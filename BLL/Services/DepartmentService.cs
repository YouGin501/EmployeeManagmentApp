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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            var result = await _repository.GetAll();
            return result;
        }
        public async Task<Department> GetById(int id)
        {
            var result = await _repository.GetById(id);
            return result;
        }

        public async Task AddDepartment(Department department)
        {
            await _repository.AddDepartment(department);
        }
        public async Task UpdateDepartment(int id, Department department)
        {
            await _repository.UpdateDepartment(id, department);
        }
        public async Task DeleteDepartment(int id)
        {
            await _repository.DeleteDepartment(id);
        }
    }
}
