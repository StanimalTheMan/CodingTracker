using CodingTracker;
using System.Data.SQLite;
using System.Configuration;
using System.Collections.Specialized;
using CodingTracker.Model;

string? userInput;
string? menuChoice;
bool isValidInput = false;
int[] menuNumbers = [0, 1, 2, 3, 4, 5];

Console.Title = "Coding Tracker";

string? dbPath = ConfigurationManager.AppSettings.Get("dbPath");

string connection = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

var repository = new CodingSessionRepository(connection);

repository.CreateTable();
repository.SeedDatabase();

do
{
    menuChoice = Display.PrintMainMenu();

    switch (menuChoice)
    {
        case "Close Application":
            Console.WriteLine("\nGoodbye!");
            Thread.Sleep(2000);
            Environment.Exit(0);
            break;
        case "View All Records":
            Display.PrintAllRecords("View All Records");
            ReturnToMainMenu();
            break;
        case "Add Record":
            RecordsController.AddRecord();
            ReturnToMainMenu();
            break;
        case "Edit Record":
            RecordsController.EditRecord();
            ReturnToMainMenu();
            break;
        case "Delete Record":
            RecordsController.DeleteRecord();
            ReturnToMainMenu();
            break;
        case "View Report":
            var reportData = repository.GetReportData();
            Display.PrintReport(reportData);
            ReturnToMainMenu();
            break;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
} while (menuChoice != "Close Application");

void ReturnToMainMenu()
{
    Console.Write("\nPress any key to return to the Main Menu...");
    Console.ReadKey();
}












