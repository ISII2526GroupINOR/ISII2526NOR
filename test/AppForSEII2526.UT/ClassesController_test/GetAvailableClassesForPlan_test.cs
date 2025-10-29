using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UT;

using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.UT.ClassesController_test
{
    public class GetAvailableClassesForPlan_test : AppForSEII25264SqliteUT
    {
        public GetAvailableClassesForPlan_test()
        {
            // TODO: Implement initial data relative to today AND weel-aligned
            var typeItems = new List<TypeItem>
            {
                new TypeItem("Bench"),
                new TypeItem("Mat"),
                new TypeItem("Dumbbell")
            };
            var classes = new List<Class>
            {
                new Class("Yoga Basics I", DateTime.Now.AddDays(8), 20, 15.00m),
                new Class("Advanced Pilates", DateTime.Now.AddDays(9), 15, 20.00m),
                new Class("Cardio Blast", DateTime.Now.AddDays(10), 25, 12.50m),
                new Class("Strength Training", DateTime.Now.AddDays(11), 30, 18.00m)
            };

            var controller = new ClassesController(_context, null);
            _context.TypeItems.AddRange(typeItems);
            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }
    }
}
