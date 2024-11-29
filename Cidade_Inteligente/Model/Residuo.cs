using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace Cidade_Inteligente.Model
{
    public class Residuo
    {
        public int IdResiduo { get; set; }
        public string Tipo { get; set; }
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; }
        public string DsCategoria { get; set; }
        public string RgSeparacao { get; set; }
    }
    public interface IResiduoRepositorio
    {
        Residuo Get(int id);
        Task<bool> Add(Residuo item);
        IEnumerable<Residuo> GetAll();
        Task<bool> Update(Residuo item);
        Task<bool> Delete(int id);
    }
}
