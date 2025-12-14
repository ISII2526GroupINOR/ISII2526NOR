using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.AuxiliaryClasses
{
    /// <summary>
    /// This static class provides a set of predefined DateOnly and TimeOnly constants with the goal of facilitating the creation of test cases that work with dates and times.
    /// It also includes a method to combine DateOnly and TimeOnly into DateTime.
    /// </summary>
    public static class TimeTable
    {
        public static readonly DateTime now = DateTime.Now;
        // Obtain and calculate some dates for the tests

        public static readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        // These dates might be useful for boundary tests
        public static readonly DateOnly nextWeekMonday = today.DayOfWeek == DayOfWeek.Sunday // Remember: Sunday = 0, Monday = 1, ..., Saturday = 6
            ? today.AddDays(1)
            : today.AddDays(8 - (int)today.DayOfWeek);
        public static readonly DateOnly nextWeekSunday = nextWeekMonday.AddDays(6);
        public static readonly DateOnly followingWeekMonday = nextWeekMonday.AddDays(7);
        public static readonly DateOnly previousWeekSunday = nextWeekMonday.AddDays(-1);

        // These dates might be useful for Equivalence Class tests
        public static readonly DateOnly endsOfTime = today.AddYears(1997); // A date far in the future
        public static readonly DateOnly middleOfNextWeek = nextWeekMonday.AddDays(3); // Middle of next week
        public static readonly DateOnly originOfTime = today.AddYears(-1997); // A date far in the past

        // Some example times
        public static readonly TimeOnly timeMorning = new TimeOnly(9, 0);              // 09:00
        public static readonly TimeOnly timeMiddleMorning = new TimeOnly(11, 30);      // 11:30
        public static readonly TimeOnly timeAfternoon = new TimeOnly(17, 0);           // 17:00
        public static readonly TimeOnly timeEvening = new TimeOnly(19, 0);             // 19:00
        public static readonly TimeOnly timeNight = new TimeOnly(22, 0);               // 22:00
        public static DateTime Combine(DateOnly d, TimeOnly t) => d.ToDateTime(t);
    }
}
