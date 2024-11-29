using MySql.Data.MySqlClient;
using System.Data;

namespace Cidade_Inteligente.Model
{
    public class FrotaRepositorio : IFrotaRepositorio
    {
        private readonly List<Frota> Frota = new();
        private readonly string connectionString;

        public FrotaRepositorio(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlDb");
            LoadFrota();
        }

        public Frota Get(int id)
        {
            return Frota.FirstOrDefault(f => f.IdCaminhao == id);
        }

        private void LoadFrota()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (var command = new MySqlCommand(
                        "SELECT id_caminhao, placa, capacidade, loc_atual FROM t_caminhao", conn))
                    {
                        using var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Frota.Add(new Frota
                            {
                                IdCaminhao = reader["id_caminhao"] != DBNull.Value ? Convert.ToInt32(reader["id_caminhao"]) : 0,
                                Placa = reader["placa"] != DBNull.Value ? reader["placa"].ToString() : "Placa Não Encontrada.",
                                Capacidade = reader["capacidade"] != DBNull.Value ? Convert.ToDouble(reader["capacidade"]) : 0,
                                LocAtual = reader["loc_atual"] != DBNull.Value ? reader["loc_atual"].ToString() : "Localizacao invalida.",
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar Frota: {ex.Message}");
                }
            }
        }

        public async Task<bool> Add(Frota item)
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var insertCommand = new MySqlCommand(
                    "INSERT INTO t_caminhao (id_caminhao, placa, capacidade, loc_atual) VALUES (@IdCaminhao, @Placa, @Capacidade, @LocAtual)", conn);
                insertCommand.Parameters.AddWithValue("@IdCaminhao", item.IdCaminhao);
                insertCommand.Parameters.AddWithValue("@Placa", item.Placa);
                insertCommand.Parameters.AddWithValue("@Capacidade", item.Capacidade);
                insertCommand.Parameters.AddWithValue("@LocAtual", item.LocAtual);

                int rowsAffected = await insertCommand.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    Frota.Add(item);
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao adicionar frota: " + ex.Message);
                return false;
            }
        }

        public IEnumerable<Frota> GetAll()
        {
            try
            {
                return Frota;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter todos os Frota: " + ex.Message);
                return Enumerable.Empty<Frota>();
            }
        }

        public async Task<bool> Update(Frota item)
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var updateCommand = new MySqlCommand(
                    "UPDATE t_caminhao SET placa = @Placa, capacidade = @Capacidade, loc_atual = @LocAtual WHERE id_caminhao = @IdCaminhao", conn);

                updateCommand.Parameters.AddWithValue("@IdCaminhao", item.IdCaminhao);
                updateCommand.Parameters.AddWithValue("@Placa", item.Placa);
                updateCommand.Parameters.AddWithValue("@Capacidade", item.Capacidade);
                updateCommand.Parameters.AddWithValue("@LocAtual", item.LocAtual);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    var existingFrota = Frota.FirstOrDefault(p => p.IdCaminhao == item.IdCaminhao);
                    if (existingFrota != null)
                    {
                        existingFrota.Placa = item.Placa;
                        existingFrota.Capacidade = item.Capacidade;
                        existingFrota.LocAtual = item.LocAtual;
                    }
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar frota: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();
                using var deleteCommand = new MySqlCommand(
                    "DELETE FROM t_caminhao WHERE id_caminhao = @IdCaminhao", conn);

                deleteCommand.Parameters.AddWithValue("@IdCaminhao", id);

                int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    var frotaToRemove = Frota.FirstOrDefault(f => f.IdCaminhao == id);
                    if (frotaToRemove != null)
                    {
                        Frota.Remove(frotaToRemove);
                    }
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar frota: " + ex.Message);
                return false;
            }
        }
        Task<bool> IFrotaRepositorio.Update(Frota item)
        {
            return Update(item);
        }
    }
}
