using BLL.Contracts;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;
        public PositionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        // GET: api/<PositionsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetAll()
        {
            var result = await _positionService.GetAll();
            return Ok(result);
        }

        // GET api/<PositionsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> Get(int id)
        {
            var result = await _positionService.GetById(id);
            return Ok(result);
        }

        // POST api/<PositionsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Position position)
        {
            await _positionService.AddPosition(position);
            return Ok();
        }

        // PUT api/<PositionsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Position position)
        {
            await _positionService.UpdatePosition(id, position);
            return Ok();
        }

        // DELETE api/<PositionsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _positionService.DeletePosition(id);
            return Ok();
        }
    }
}
