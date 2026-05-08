using Dapper;
using FitnessStudio.Data.Database;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Repositories
{
    public class UplataRepository : IUplataRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;

        public UplataRepository(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Uplata> GetByClanarinaId(int clanarinaId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    uplata_id AS UplataId,
                    clanarina_id AS ClanarinaId,
                    iznos AS Iznos,
                    datum::timestamp AS Datum
                FROM Uplata
                WHERE clanarina_id = @ClanarinaId
                ORDER BY datum;
                """;

            return connection.Query<Uplata>(sql, new { ClanarinaId = clanarinaId });
        }

        public Uplata? GetById(int uplataId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    uplata_id AS UplataId,
                    clanarina_id AS ClanarinaId,
                    iznos AS Iznos,
                    datum::timestamp AS Datum
                FROM Uplata
                WHERE uplata_id = @UplataId;
                """;

            return connection.QueryFirstOrDefault<Uplata>(sql, new { UplataId = uplataId });
        }

        public void Insert(Uplata uplata)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                INSERT INTO Uplata (clanarina_id, iznos, datum)
                VALUES (@ClanarinaId, @Iznos, @Datum);
                """;

            connection.Execute(sql, uplata);
        }

        public void Update(Uplata uplata)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                UPDATE Uplata
                SET iznos = @Iznos,
                    datum = @Datum
                WHERE uplata_id = @UplataId;
                """;

            connection.Execute(sql, uplata);
        }

        public void Delete(int uplataId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                DELETE FROM Uplata
                WHERE uplata_id = @UplataId;
                """;

            connection.Execute(sql, new { UplataId = uplataId });
        }

        public decimal GetTotalForClanarina(int clanarinaId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT COALESCE(SUM(iznos), 0)
                FROM Uplata
                WHERE clanarina_id = @ClanarinaId;
                """;

            return connection.ExecuteScalar<decimal>(sql, new { ClanarinaId = clanarinaId });
        }
    }
}