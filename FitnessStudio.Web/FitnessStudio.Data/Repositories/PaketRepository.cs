using Dapper;
using FitnessStudio.Data.Database;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Repositories
{
    public class PaketRepository : IPaketRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;

        public PaketRepository(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Paket> GetAll()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT 
                    paket_id AS PaketId,
                    naziv AS Naziv,
                    broj_treninga AS BrojTreninga,
                    cijena AS Cijena
                FROM Paket
                ORDER BY naziv;
                """;

            return connection.Query<Paket>(sql);
        }

        public Paket? GetById(int paketId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT 
                    paket_id AS PaketId,
                    naziv AS Naziv,
                    broj_treninga AS BrojTreninga,
                    cijena AS Cijena
                FROM Paket
                WHERE paket_id = @PaketId;
                """;

            return connection.QueryFirstOrDefault<Paket>(sql, new { PaketId = paketId });
        }

        public IEnumerable<Paket> SearchByName(string? naziv)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT 
                    paket_id AS PaketId,
                    naziv AS Naziv,
                    broj_treninga AS BrojTreninga,
                    cijena AS Cijena
                FROM Paket
                WHERE @Naziv IS NULL 
                OR @Naziv = '' 
                OR naziv ILIKE '%' || @Naziv || '%'
                ORDER BY naziv;
                """;

            return connection.Query<Paket>(sql, new { Naziv = naziv });
        }

        public void Insert(Paket paket)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                INSERT INTO Paket (naziv, broj_treninga, cijena)
                VALUES (@Naziv, @BrojTreninga, @Cijena);
                """;

            connection.Execute(sql, paket);
        }

        public void Update(Paket paket)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                UPDATE Paket
                SET naziv = @Naziv,
                    broj_treninga = @BrojTreninga,
                    cijena = @Cijena
                WHERE paket_id = @PaketId;
                """;

            connection.Execute(sql, paket);
        }

        public void Delete(int paketId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                DELETE FROM Paket
                WHERE paket_id = @PaketId;
                """;

            connection.Execute(sql, new { PaketId = paketId });
        }

        public bool ExistsByName(string naziv)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT COUNT(1)
                FROM Paket
                WHERE LOWER(naziv) = LOWER(@Naziv);
                """;

            return connection.ExecuteScalar<int>(sql, new { Naziv = naziv }) > 0;
        }

        public bool ExistsByName(string naziv, int paketId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT COUNT(1)
                FROM Paket
                WHERE LOWER(naziv) = LOWER(@Naziv)
                  AND paket_id <> @PaketId;
                """;

            return connection.ExecuteScalar<int>(sql, new { Naziv = naziv, PaketId = paketId }) > 0;
        }
    }
}