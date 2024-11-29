using Microsoft.AspNetCore.Mvc;
using Cidade_Inteligente.Model;

namespace Cidade_Inteligente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResiduoController : ControllerBase
    {
        private readonly IResiduoRepositorio _repositorio;

        public ResiduoController(IResiduoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10) => Ok(_repositorio.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var residuo = _repositorio.Get(id);
            return residuo == null ? NotFound() : Ok(residuo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Residuo residuo)
        {
            if (residuo == null)
                return BadRequest();

            var created = await _repositorio.Add(residuo);
            return created ? CreatedAtAction(nameof(Get), new { id = residuo.IdResiduo }, residuo) : StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Residuo residuo)
        {
            if (residuo == null || residuo.IdResiduo != id)
                return BadRequest();

            var updated = await _repositorio.Update(residuo);
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
