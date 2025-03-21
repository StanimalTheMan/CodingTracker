using System.Configuration;
using System.Data.SQLite;
using System.Globalization;
using Dapper;
using System;

namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;

        public CodingSession() { }

        public CodingSession(DateTime date, DateTime startTime, DateTime endTime)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int GetRecordsCount(SQLiteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM CodingSessions";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }



                //public Dictionary<string, int> CalculateStreak()
                //{
                //    string? dbPath = ConfigurationManager.AppSettings.Get("dbPath");

                //    using (var connection = new SQLiteConnection($"Data Source={dbPath}"))
                //    {
                //        connection.Open();

                //        var longestStreaks = new Dictionary<string, int>();

                //        string query = @"
                //SELECT h.Habit, hi.Date
                //FROM HabitInstances hi
                //JOIN Habits h ON hi.HabitId = h.Id
                //ORDER BY h.Habit, hi.Date";

                //        using (var command = new SQLiteCommand(query, connection))
                //        using (var reader = command.ExecuteReader())
                //        {
                //            string? currentHabit = null;
                //            int currentStreak = 0;
                //            int longestStreak = 0;
                //            DateTime? lastDate = null;

                //            while (reader.Read())
                //            {
                //                string? habit = reader["Habit"].ToString();
                //                DateTime date = DateTime.Parse(reader["Date"].ToString(), CultureInfo.InvariantCulture);

                //                if (currentHabit != habit)
                //                {
                //                    if (currentHabit != null)
                //                    {
                //                        longestStreaks[currentHabit] = Math.Max(longestStreaks.GetValueOrDefault(currentHabit, 0), longestStreak);
                //                    }

                //                    currentHabit = habit;
                //                    currentStreak = 1;
                //                    longestStreak = 1;
                //                    lastDate = date;
                //                }
                //                else
                //                {
                //                    if (lastDate.HasValue && (date - lastDate.Value).Days == 1)
                //                    {
                //                        currentStreak++;
                //                    }
                //                    else
                //                    {
                //                        currentStreak = 1;
                //                    }

                //                    longestStreak = Math.Max(longestStreak, currentStreak);
                //                }

                //                lastDate = date;
                //            }

                //            if (currentHabit != null)
                //            {
                //                longestStreaks[currentHabit] = Math.Max(longestStreaks.GetValueOrDefault(currentHabit, 0), longestStreak);
                //            }
                //        }

                //        return longestStreaks;
                //    }
                //}

            
    }
}