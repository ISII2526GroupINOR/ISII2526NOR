using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.UT.RestocksController_test
{
    public class CreateRestock_test : AppForSEII25264SqliteUT
    {
        public CreateRestock_test()
        {
            var brand = new Brand("Precor");
            var typeItem = new TypeItem("Dumbbell");
            var items = new List<Item>
            {
                new Item("Dumbbell", "Regular dumbbell", 0, 8, 6, 30, brand, typeItem),
                new Item("Kettlebell", "Cicular dumbbell", 0, 5, 3, 40, brand, typeItem)
            };

            _context.AddRange(items);
            _context.SaveChanges();

            var user = new ApplicationUser("Jaime", "Domingo");

            var restock = new Restock("A restock", "An address", "A description", new DateTime(), new DateTime(),
                180, new List<RestockItem>(), user);
            restock.RestockItems.Add(new RestockItem(2, new Restock(), items[0]));
            restock.RestockItems.Add(new RestockItem(3, new Restock(), items[1]));

            _context.Users.Add(user);
            _context.Add(restock);
            _context.SaveChanges();
        }
    }
}
