using Microsoft.AspNetCore.Mvc;
using Cidade_Inteligente.Model;

namespace Cidade_Inteligente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrotaController : ControllerBase
    {
        private readonly IFrotaRepositorio _frotaRepositorio;

        public FrotaController(IFrotaRepositorio frotaRepositorio)
        {
            _frotaRepositorio = frotaRepositorio;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10) => Ok(_frotaRepositorio.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var frota = _frotaRepositorio.Get(id);
            return frota == null ? NotFound() : Ok(frota);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Frota frota)
        {
            if (frota == null)
                return BadRequest("O objeto Frota não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Retorna os erros de validação do modelo.

            try
            {
                var created = await _frotaRepositorio.Add(frota);
                if (created)
                {
                    return CreatedAtAction(nameof(Get), new { id = frota.IdCaminhao }, frota);
                }
                else
                {
                    return StatusCode(500, "Erro ao salvar a frota no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Log do erro para rastreamento
                Console.WriteLine($"Erro ao criar frota: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro interno ao processar a solicitação.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Frota frota)
        {
            if (frota == null || frota.IdCaminhao != id)
                return BadRequest();

            var updated = await _frotaRepositorio.Update(frota);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _frotaRepositorio.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
