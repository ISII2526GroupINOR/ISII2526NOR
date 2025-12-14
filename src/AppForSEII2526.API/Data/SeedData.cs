using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.AuxiliaryClasses;

namespace AppForSEII2526.API.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
        {
            // Update class dates - This assumes that the database is already populated (seeded)
            try
            {
                UpdateClassDates(dbContext);
                logger.LogInformation("Class dates updated successfully to next week.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating class dates.");
            }
        }

        public static void UpdateClassDates(ApplicationDbContext dbContext)
        {
            // Ensure database is created
            dbContext.Database.EnsureCreated();


            int numberOfUpdatedClasses = 4; // For easier modification later
            var classesToUpdate = dbContext.Classes.Where(c => c.Id >= 1 && c.Id <= numberOfUpdatedClasses).ToList();

            // Set new dates manually
            classesToUpdate[0].Date = TimeTable.Combine(TimeTable.nextWeekMonday,       TimeTable.timeMorning);
            classesToUpdate[1].Date = TimeTable.Combine(TimeTable.middleOfNextWeek,     TimeTable.timeAfternoon);
            classesToUpdate[2].Date = TimeTable.Combine(TimeTable.nextWeekSunday,       TimeTable.timeEvening);
            classesToUpdate[3].Date = TimeTable.Combine(TimeTable.followingWeekMonday,  TimeTable.timeNight); // Should be unfiltered

            // Save changes to the database
            dbContext.SaveChanges();

        }
    }
}
