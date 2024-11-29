using Microsoft.AspNetCore.Mvc;
using Cidade_Inteligente.Model;

namespace Cidade_Inteligente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoRepositorio _repositorio;

        public NotificacaoController(INotificacaoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10) => Ok(_repositorio.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var notificacao = _repositorio.Get(id);
            return notificacao == null ? NotFound() : Ok(notificacao);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notificacao notificacao)
        {
            if (notificacao == null)
                return BadRequest();

            var created = await _repositorio.Add(notificacao);
            return created ? CreatedAtAction(nameof(Get), new { id = notificacao.IdNotificacao }, notificacao) : StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Notificacao notificacao)
        {
            if (notificacao == null || notificacao.IdNotificacao != id)
                return BadRequest();

            var updated = await _repositorio.Update(notificacao);
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
