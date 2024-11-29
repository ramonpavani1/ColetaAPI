using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace Cidade_Inteligente.Model
{
    public class Recipiente
    {
        public double IdRecipiente { set; get; }
        public string Localizacao { get; set; }
        public double CapTotal { get; set; }
        public double CapAtual { get; set; }
    }

    public interface IRecipienteRepositorio
    {
        Recipiente Get(int id);
        Task<bool> Add(Recipiente item);
        IEnumerable<Recipiente> GetAll();
        Task<bool> Update(Recipiente item);
        Task<bool> Delete(int id);
    }
}
