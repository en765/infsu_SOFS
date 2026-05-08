using Dapper;
using FitnessStudio.Data.Database;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Repositories
{
    public class ClanarinaRepository : IClanarinaRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;

        public ClanarinaRepository(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Clanarina> GetAll()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    clanarina_id AS ClanarinaId,
                    clan_id AS ClanId,
                    paket_id AS PaketId,
                    datum_pocetka::timestamp AS DatumPocetka,
                    datum_zavrsetka::timestamp AS DatumZavrsetka,
                    status AS Status
                FROM Clanarina
                ORDER BY datum_pocetka DESC;
                """;

            return connection.Query<Clanarina>(sql);
        }

        public Clanarina? GetById(int clanarinaId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    clanarina_id AS ClanarinaId,
                    clan_id AS ClanId,
                    paket_id AS PaketId,
                    datum_pocetka::timestamp AS DatumPocetka,
                    datum_zavrsetka::timestamp AS DatumZavrsetka,
                    status AS Status
                FROM Clanarina
                WHERE clanarina_id = @ClanarinaId;
                """;

            return connection.QueryFirstOrDefault<Clanarina>(sql, new { ClanarinaId = clanarinaId });
        }

        public IEnumerable<Clanarina> Search(string? pojam)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    c.clanarina_id AS ClanarinaId,
                    c.clan_id AS ClanId,
                    c.paket_id AS PaketId,
                    c.datum_pocetka::timestamp AS DatumPocetka,
                    c.datum_zavrsetka::timestamp AS DatumZavrsetka,
                    c.status AS Status
                FROM Clanarina c
                JOIN Clan cl ON c.clan_id = cl.clan_id
                JOIN Paket p ON c.paket_id = p.paket_id
                WHERE @Pojam IS NULL
                   OR @Pojam = ''
                   OR cl.ime ILIKE '%' || @Pojam || '%'
                   OR cl.prezime ILIKE '%' || @Pojam || '%'
                   OR p.naziv ILIKE '%' || @Pojam || '%'
                   OR c.status ILIKE '%' || @Pojam || '%'
                ORDER BY c.datum_pocetka DESC;
                """;

            return connection.Query<Clanarina>(sql, new { Pojam = pojam });
        }

        public void Insert(Clanarina clanarina)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                INSERT INTO Clanarina (clan_id, paket_id, datum_pocetka, datum_zavrsetka, status)
                VALUES (@ClanId, @PaketId, @DatumPocetka, @DatumZavrsetka, @Status);
                """;

            connection.Execute(sql, new
            {
                clanarina.ClanId,
                clanarina.PaketId,
                clanarina.DatumPocetka,
                clanarina.DatumZavrsetka,
                Status = clanarina.Status.ToString()
            });
        }

        public void Update(Clanarina clanarina)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                UPDATE Clanarina
                SET clan_id = @ClanId,
                    paket_id = @PaketId,
                    datum_pocetka = @DatumPocetka,
                    datum_zavrsetka = @DatumZavrsetka,
                    status = @Status
                WHERE clanarina_id = @ClanarinaId;
                """;

            connection.Execute(sql, new
            {
                clanarina.ClanarinaId,
                clanarina.ClanId,
                clanarina.PaketId,
                clanarina.DatumPocetka,
                clanarina.DatumZavrsetka,
                Status = clanarina.Status.ToString()
            });
        }

        public void Delete(int clanarinaId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                DELETE FROM Clanarina
                WHERE clanarina_id = @ClanarinaId;
                """;

            connection.Execute(sql, new { ClanarinaId = clanarinaId });
        }
    }
}