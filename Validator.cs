using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingTracker;

public static class Validator
{
    public static DateTime GetValidDate(string prompt, bool isTime)
    {
        do
        {
            Console.Write("\n" + prompt);
            string userInput = Console.ReadLine();
            DateTime? date = ValidateDate(userInput, isTime);
            
            if (date.HasValue)
            {
                return date.Value;
            } 
            else
            {
                Console.WriteLine($"Invalid date format.  {prompt}");
            }
        } while (true);
    }

    public static DateTime? ValidateDate(string userInput, bool isTime)
    {
        if (isTime)
        {
            // asking for start or end time since author of this code is using DateTime and not Timespan for start and end time for a coding session
            if (DateTime.TryParseExact(userInput, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime time))
            {
                return time;
            }
        } else
        {
            // asking for the date of coding session
            if (DateTime.TryParseExact(userInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
        }

        return null;
    }
}
