using System.Configuration;
using System.Data.SQLite;
using Dapper;

namespace CodingTracker.Model
{
    public class CodingSessionRepository
    {
        private readonly string _connection;

        string? dbPath = ConfigurationManager.ConnectionStrings["connection"]?.ConnectionString;

        public CodingSessionRepository(string connection)
        {
            _connection = connection;
        }

        public void CreateTable()
        {
            using (var dbConnection = new SQLiteConnection(_connection))
            {
                string createTableSql = @"
                    CREATE TABLE IF NOT EXISTS CodingSessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date DATE NOT NULL,
                        StartTime DATETIME NOT NULL,
                        EndTime DATETIME NOT NULL                
                    );";

                dbConnection.Execute(createTableSql);
            }
        }

        public List<CodingSession> GetAllCodingSessions()
        {
            var sessions = new List<CodingSession>();

            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                string query = @"SELECT * FROM CodingSessions ORDER BY Date DESC";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var session = new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            StartTime = reader.GetDateTime(2),
                            EndTime = reader.GetDateTime(3)
                        };
                        sessions.Add(session);
                    }
                }
            }

            return sessions;
        }

        public (int TotalDistinctDays, int TotalSessions, string TotalDuration) GetReportData()
        {
            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                string query = @"
            SELECT 
                COUNT(DISTINCT Date) AS TotalDistinctDays,
                COUNT(*) AS TotalSessions,
                SUM(julianday(EndTime) - julianday(StartTime)) * 24 * 60 * 60 AS TotalDurationInSeconds
            FROM 
                CodingSessions;";

                var result = connection.QuerySingle(query);

                // Handle potential null values and cast to int
                int totalSeconds = (int)(result.TotalDurationInSeconds ?? 0); // Default to 0 if null
                int totalDistinctDays = (int)result.TotalDistinctDays;
                int totalSessions = (int)result.TotalSessions;

                TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
                string formattedDuration = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    (int)timeSpan.TotalHours,
                    timeSpan.Minutes,
                    timeSpan.Seconds);

                return (
                    TotalDistinctDays: totalDistinctDays,
                    TotalSessions: totalSessions,
                    TotalDuration: formattedDuration
                );
            }
        }

        public void SeedDatabase()
        {
            using (var dbConnection = new SQLiteConnection(_connection))
            {
                dbConnection.Open();
                // Check if there are any existing records
                var existingSessions = dbConnection.QuerySingleOrDefault<int>("SELECT COUNT(*) FROM CodingSessions;");

                if (existingSessions == 0)
                {
                    // Create five default coding sessions
                    var sessions = new List<CodingSession>
                    {
                        new CodingSession(new DateTime(2023, 10, 1), new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 10, 0, 0)),
                        new CodingSession(new DateTime(2023, 10, 2), new DateTime(2023, 10, 2, 11, 0, 0), new DateTime(2023, 10, 2, 12, 30, 0)),      
                        new CodingSession(new DateTime(2023, 10, 3), new DateTime(2023, 10, 3, 14, 0, 0), new DateTime(2023, 10, 3, 15, 15, 0)),
                        new CodingSession(new DateTime(2023, 10, 4), new DateTime(2023, 10, 4, 16, 0, 0), new DateTime(2023, 10, 4, 17, 0, 0)),
                        new CodingSession(new DateTime(2023, 10, 5), new DateTime(2023, 10, 5, 18, 0, 0), new DateTime(2023, 10, 5, 19, 30, 0))
                    };

                    // Insert each session into the database
                    foreach (var session in sessions)
                    {
                        string insertSql = @"
                            INSERT INTO CodingSessions (Date, StartTime, EndTime)
                            VALUES (@Date, @StartTime, @EndTime);";

                        dbConnection.Execute(insertSql, new { Date = session.Date, StartTime = session.StartTime, EndTime = session.EndTime });
                    }
                }
            }
        }
    }
}