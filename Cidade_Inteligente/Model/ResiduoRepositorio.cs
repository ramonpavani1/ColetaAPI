using MySql.Data.MySqlClient;
using System.Data;

namespace Cidade_Inteligente.Model
{
    public class ResiduoRepositorio : IResiduoRepositorio
    {
        private readonly List<Residuo> Residuos = new();
        private readonly string connectionString;

        public ResiduoRepositorio(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlDb");
            LoadResiduos();
        }

        public IEnumerable<Residuo> GetAll() => Residuos;

        public Residuo Get(int id) => Residuos.FirstOrDefault(n => n.IdResiduo == id);

        public async Task<bool> Add(Residuo Residuo)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(
                "INSERT INTO t_tp_residuo (tipo, qtde, unidade, ds_categoria, rg_separacao) VALUES (:Tipo, :Quantidade, :Unidade, :DsCategoria, :RgSepagarao)", conn);

            cmd.Parameters.Add(new MySqlParameter("IdResiduo", Residuo.IdResiduo));
            cmd.Parameters.Add(new MySqlParameter("DsCategoria", Residuo.DsCategoria));
            cmd.Parameters.Add(new MySqlParameter("RgSeparacao", Residuo.RgSeparacao));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                Residuos.Add(Residuo);

            return rowsAffected > 0;
        }

        public async Task<bool> Update(Residuo Residuo)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(
                "UPDATE t_tp_residuo SET tipo = :Tipo, qtde = :Quantidade, unidade = :Unidade, ds_categoria = :DsCategoria, rg_separacao= :RgCategoria WHERE id_Residuo = :IdResiduo", conn);

            cmd.Parameters.Add(new MySqlParameter("DsCategoria", Residuo.DsCategoria));
            cmd.Parameters.Add(new MySqlParameter("RgSeparacao", Residuo.RgSeparacao));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                var existing = Residuos.FirstOrDefault(n => n.IdResiduo == Residuo.IdResiduo);
                if (existing != null)
                {
                    existing.DsCategoria = Residuo.DsCategoria;
                    existing.RgSeparacao = Residuo.RgSeparacao;
                }
            }

            return rowsAffected > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("DELETE FROM t_tp_residuo WHERE id_Residuo = :IdResiduo", conn);
            cmd.Parameters.Add(new MySqlParameter("IdResiduo", id));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                Residuos.RemoveAll(n => n.IdResiduo == id);

            return rowsAffected > 0;
        }

        private void LoadResiduos()
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id_Residuo, unidade, qtde, tipo, ds_categoria, rg_separacao FROM t_tp_residuo", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Residuos.Add(new Residuo
                {
                    IdResiduo = reader["id_Residuo"] != DBNull.Value ?  Convert.ToInt32(reader["id_Residuo"]): 0,
                    DsCategoria = reader["ds_categoria"] != DBNull.Value ? reader["ds_categoria"].ToString(): "Categoria Invalida",
                    RgSeparacao = reader["rg_separacao"] != DBNull.Value ? reader["rg_separacao"].ToString() : "Separação Invalida"
                });
            }
        }
    }
}

