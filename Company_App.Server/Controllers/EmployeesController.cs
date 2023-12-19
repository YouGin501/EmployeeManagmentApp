using BLL.Contracts;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        // GET: api/<EmployeesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            var result = await _employeeService.GetAll();
            return Ok(result);
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            var result = await _employeeService.GetById(id);
            return Ok(result);
        }

        // POST api/<EmployeesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            await _employeeService.AddEmployee(employee);
            return Ok();
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Employee employee)
        {
            await _employeeService.UpdateEmployee(id, employee);
            return Ok();
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteEmployee(id);
            return Ok(); 
        }

        [HttpGet]
        [Route("GeneralSalaryInfo")]
        public async Task<ActionResult<IEnumerable<Employee>>> GeneralSalaryInfo(DateTime startPeriod, DateTime endPeriod, int? departmentId, int? positionId)
        {
            var result = await _employeeService.GeneralSalaryInfo(startPeriod, endPeriod, departmentId, positionId);
            return Ok(result);
        }
    }
}
