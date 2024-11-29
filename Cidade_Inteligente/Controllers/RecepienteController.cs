using Microsoft.AspNetCore.Mvc;
using Cidade_Inteligente.Model;

namespace Cidade_Inteligente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipienteController : ControllerBase
    {
        private readonly IRecipienteRepositorio _repositorio;

        public RecipienteController(IRecipienteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10) => Ok(_repositorio.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var recipiente = _repositorio.Get(id);
            return recipiente == null ? NotFound() : Ok(recipiente);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Recipiente recipiente)
        {
            if (recipiente == null)
                return BadRequest();

            var created = await _repositorio.Add(recipiente);
            return created ? CreatedAtAction(nameof(Get), new { id = recipiente.IdRecipiente }, recipiente) : StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Recipiente recipiente)
        {
            if (recipiente == null || recipiente.IdRecipiente != id)
                return BadRequest();

            var updated = await _repositorio.Update(recipiente);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repositorio.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
