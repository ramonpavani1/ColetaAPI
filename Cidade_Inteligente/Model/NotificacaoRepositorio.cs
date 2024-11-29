using System.Data;
using MySql.Data.MySqlClient;

namespace Cidade_Inteligente.Model
{
    public class NotificacaoRepositorio : INotificacaoRepositorio
    {
        private readonly List<Notificacao> Notificacoes = new();
        private readonly string connectionString;

        public NotificacaoRepositorio(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlDb");
            LoadNotificacoes();
        }

        public IEnumerable<Notificacao> GetAll() => Notificacoes;

        public Notificacao Get(int id) => Notificacoes.FirstOrDefault(n => n.IdNotificacao == id);

        public async Task<bool> Add(Notificacao notificacao)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(
                "INSERT INTO t_notificacao (titulo, mensagem, data_criacao) VALUES (:Titulo, :Mensagem, :DataCriacao)", conn);

            cmd.Parameters.Add(new MySqlParameter("Titulo", notificacao.Titulo));
            cmd.Parameters.Add(new MySqlParameter("Mensagem", notificacao.Mensagem));
            cmd.Parameters.Add(new MySqlParameter("DataCriacao", notificacao.DataCriacao));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                Notificacoes.Add(notificacao);

            return rowsAffected > 0;
        }

        public async Task<bool> Update(Notificacao notificacao)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand(
                "UPDATE t_notificacao SET titulo = :Titulo, mensagem = :Mensagem, data_criacao = :DataCriacao WHERE id_notificacao = :IdNotificacao", conn);

            cmd.Parameters.Add(new MySqlParameter("Titulo", notificacao.Titulo));
            cmd.Parameters.Add(new MySqlParameter("Mensagem", notificacao.Mensagem));
            cmd.Parameters.Add(new MySqlParameter("DataCriacao", notificacao.DataCriacao));
            cmd.Parameters.Add(new MySqlParameter("IdNotificacao", notificacao.IdNotificacao));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                var existing = Notificacoes.FirstOrDefault(n => n.IdNotificacao == notificacao.IdNotificacao);
                if (existing != null)
                {
                    existing.Titulo = notificacao.Titulo;
                    existing.Mensagem = notificacao.Mensagem;
                    existing.DataCriacao = notificacao.DataCriacao;
                }
            }

            return rowsAffected > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("DELETE FROM t_notificacao WHERE id_notificacao = :IdNotificacao", conn);
            cmd.Parameters.Add(new MySqlParameter("IdNotificacao", id));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                Notificacoes.RemoveAll(n => n.IdNotificacao == id);

            return rowsAffected > 0;
        }

        private void LoadNotificacoes()
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id_notificacao, titulo, mensagem, data_criacao FROM t_notificacao", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Notificacoes.Add(new Notificacao
                {
                    IdNotificacao = Convert.ToInt32(reader["id_notificacao"]),
                    Titulo = reader["titulo"].ToString(),
                    Mensagem = reader["mensagem"].ToString(),
                    DataCriacao = Convert.ToDateTime(reader["data_criacao"])
                });
            }
        }
    }
}
