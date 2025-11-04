using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;

namespace AppForSEII2526.UT.RestocksController_test
{
    internal class GestRestock_test : AppForSEII25264SqliteUT
    {
        public GestRestock_test()
        {
            var items = new List<Item>
            {
                new Item()
            };
            var restockItems = new List<RestockItem> { 
                new RestockItem()
            };
            var user = new ApplicationUser("Jaime", "Domingo", null, null, new List<Restock> { });
        }
    }
}
