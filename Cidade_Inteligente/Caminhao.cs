namespace CidadeInteligente.Models
{
    public class Caminhao
    {
        public int IdCaminhao { get; set; }
        public string Placa { get; set; }
        public string LocalizacaoAtual { get; set; }
        public double Capacidade { get; set; }
        public double CargaAtual { get; set; }
        public string Status { get; internal set; }
    }
}
