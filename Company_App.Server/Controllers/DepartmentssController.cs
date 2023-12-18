using BLL.Contracts;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentssController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentssController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/<DepartmentsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll()
        {
            var result = await _departmentService.GetAll();
            return Ok(result);
        }

        // GET api/<DepartmentsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> Get(int id)
        {
            var res = await _departmentService.GetById(id);
            return Ok(res);
        }

        // POST api/<DepartmentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            await _departmentService.AddDepartment(department);
            return Ok();
        }

        // PUT api/<DepartmentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Department department)
        {
            try
            {
                await _departmentService.UpdateDepartment(id, department);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<DepartmentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentService.DeleteDepartment(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
