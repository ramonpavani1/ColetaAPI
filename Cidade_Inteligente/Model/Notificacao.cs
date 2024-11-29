using System.Collections.Generic;
using System.Threading.Tasks;


namespace Cidade_Inteligente.Model
{
    public class Notificacao
    {
        public int IdNotificacao { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataCriacao { get; set; }
    }
    public interface INotificacaoRepositorio
    {
        Notificacao Get(int id);
        Task<bool> Add(Notificacao item);
        IEnumerable<Notificacao> GetAll();
        Task<bool> Update(Notificacao item);
        Task<bool> Delete(int id);
    }
}
