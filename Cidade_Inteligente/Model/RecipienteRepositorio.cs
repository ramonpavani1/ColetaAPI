using MySql.Data.MySqlClient;

namespace Cidade_Inteligente.Model
{
    public class RecipienteRepositorio : IRecipienteRepositorio
    {
        private readonly List<Recipiente> Recipientes = new();
        private readonly string connectionString;

        public RecipienteRepositorio(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlDb");
            LoadRecipientes();
        }

        public IEnumerable<Recipiente> GetAll() => Recipientes;

        public Recipiente Get(int id) => Recipientes.FirstOrDefault(r => r.IdRecipiente == id);

        public async Task<bool> Add(Recipiente recipiente)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(
                "INSERT INTO t_recipiente (cap_total, localizacao, cap_atual) VALUES (: CapTotal, :Localizacao, :CapAtual)", conn);

            cmd.Parameters.Add(new MySqlParameter("Localizacao", recipiente. Localizacao ));
            cmd.Parameters.Add(new MySqlParameter("CapTotal", recipiente.CapTotal));
            cmd.Parameters.Add(new MySqlParameter("CapAtual", recipiente.CapAtual));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                Recipientes.Add(recipiente);

            return rowsAffected > 0;
        }

        public async Task<bool> Update(Recipiente recipiente)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(
                "UPDATE t_recipiente SET  localizacao  = : Localizacao , cap_Total = :CapTotal, cap_atual = :CapAtual WHERE id_recipiente = :IdRecipiente", conn);

            cmd.Parameters.Add(new MySqlParameter("Localizacao", recipiente.Localizacao));
            cmd.Parameters.Add(new MySqlParameter("CapTotal", recipiente.CapTotal));
            cmd.Parameters.Add(new MySqlParameter("CapAtual", recipiente.CapAtual));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                var existing = Recipientes.FirstOrDefault(r => r.IdRecipiente == recipiente.IdRecipiente);
                if (existing != null)
                {
                    existing.Localizacao  = recipiente. Localizacao ;
                    existing.CapTotal = recipiente.CapTotal;
                    existing.CapAtual = recipiente.CapAtual;
                }
            }

            return rowsAffected > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("DELETE FROM t_recipiente WHERE id_recipiente = :IdRecipiente", conn);
            cmd.Parameters.Add(new MySqlParameter("IdRecipiente", id));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                Recipientes.RemoveAll(r => r.IdRecipiente == id);

            return rowsAffected > 0;
        }

        private void LoadRecipientes()
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id_recipiente, localizacao , cap_total, cap_atual FROM t_recipiente", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Recipientes.Add(new Recipiente
                {
                    IdRecipiente = reader["id_recipiente"] != DBNull.Value ? Convert.ToInt32(reader["id_recipiente"]) : 0,
                    Localizacao = reader["localizacao"] != DBNull.Value ? reader["localizacao"].ToString() : "Localização Incorreta.",
                    CapTotal = reader["cap_total"] != DBNull.Value ? Convert.ToDouble(reader["cap_total"]): 0,
                    CapAtual = reader["cap_atual"] != DBNull.Value ? Convert.ToDouble(reader["cap_atual"]) : 0
                });
            }
        }
    }
}
