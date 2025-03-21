using System.Configuration;
using System.Data.SQLite;
using CodingTracker.Model;
using Spectre.Console;

namespace CodingTracker
{
    public static class Display
    {

        public static string PrintMainMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Close Application", "View All Records", "Add Record", "Edit Record", "Delete Record", "View Report"
                }));

            return menuChoice;
        }


        public static void PrintReport((int TotalDistinctDays, int TotalSessions, string TotalDuration) reportData)
        {
            var repository = new CodingSessionRepository(ConfigurationManager.AppSettings.Get("dbPath"));
            var sessions = repository.GetAllCodingSessions();

            // Clear the console
            Console.Clear();

            // Print a simple heading
            var rule = new Rule("[green]View Report[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

                      var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn(new TableColumn("[dodgerBlue1]Total Days[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerBlue1]Total Sessions[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerBlue1]Total Duration[/]").Centered());

            table.AddRow(
                reportData.TotalDistinctDays.ToString(),
                reportData.TotalSessions.ToString(),
                reportData.TotalDuration.ToString());
   
                    //var longestStreaks = codingSession.CalculateStreak();
                //    Console.WriteLine($"\nLongest Streaks:");
                //    //foreach (var streak in longestStreaks)
                //    {
                //        //Console.WriteLine($"{streak.Key} - {streak.Value} days in a row");
                //    }
                //}
            
        }


        public static void PrintAllRecords(string heading)
        {
            var repository = new CodingSessionRepository(ConfigurationManager.AppSettings.Get("dbPath"));
            var sessions = repository.GetAllCodingSessions();

            // Clear the console
            Console.Clear();

            // Print a simple heading
            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            // Create a table
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Date[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Start Time[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]End Time[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Duration[/]").Centered());

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No records found.[/]");
                return;
            }

            // Add rows to the table from the CodingSession objects
            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.Date.ToString("yyyy-MM-dd"),
                    session.StartTime.ToString("hh:mm tt"),
                    session.EndTime.ToString("hh:mm tt"),
                    session.Duration.ToString(@"hh\:mm") // Format duration as hours and minutes
                );
            }

            // Render the table
            AnsiConsole.Write(table);
        }
    }
        }
    
