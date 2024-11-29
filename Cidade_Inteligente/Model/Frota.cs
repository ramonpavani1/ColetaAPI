using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace Cidade_Inteligente.Model
{
    public class Frota
    {
        public int IdCaminhao { get; set; }
        public string Placa { get; set; }
        public double Capacidade { get; set; }
        public string LocAtual { get; set; }
    }

    public interface IFrotaRepositorio
    {
        Frota Get(int id);
        Task<bool> Add(Frota item);
        IEnumerable<Frota> GetAll();
        Task<bool> Update(Frota item);
        Task<bool> Delete(int id);
    }
}
