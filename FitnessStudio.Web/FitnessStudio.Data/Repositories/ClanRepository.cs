using Dapper;
using FitnessStudio.Data.Database;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Repositories
{
    public class ClanRepository : IClanRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;

        public ClanRepository(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Clan> GetAll()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    clan_id AS ClanId,
                    ime AS Ime,
                    prezime AS Prezime,
                    email AS Email,
                    telefon AS Telefon
                FROM Clan
                ORDER BY prezime, ime;
                """;

            return connection.Query<Clan>(sql);
        }

        public Clan? GetById(int clanId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    clan_id AS ClanId,
                    ime AS Ime,
                    prezime AS Prezime,
                    email AS Email,
                    telefon AS Telefon
                FROM Clan
                WHERE clan_id = @ClanId;
                """;

            return connection.QueryFirstOrDefault<Clan>(sql, new { ClanId = clanId });
        }
    }
}