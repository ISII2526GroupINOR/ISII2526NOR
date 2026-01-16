using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.Maui.Services
{
    public class POSItem
    {
        public POSItem() { }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Count { get; set; }
    }

    public class POSStateContainer
    {
        public List<POSItem> Items { get; } = new List<POSItem>();

        public POSStateContainer()
        {
        }

        public POSItem getItemByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var search = name.Trim();
            return Items.FirstOrDefault(i =>
                !string.IsNullOrEmpty(i?.Name) &&
                string.Equals(i.Name.Trim(), search, StringComparison.OrdinalIgnoreCase));
        }
        public int getIndexByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return -1;


            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                if (item.Name == name) return i;
            }
            return -1;
        }

        public void AddItem(string name, int ammount, string description = "", decimal UnitPrice = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            // Check if name already exists in the list
            int index = getIndexByName(name);
            if(index >= 0)
            {
                Items[index].Count += ammount;
                return;
            }
            
            Items.Add( new POSItem { Name = name, Count = 1, Description = description, UnitPrice = UnitPrice });
        }


        public bool AddItemByCode(string code)
        {
            if(string.IsNullOrWhiteSpace(code)) return false;

            if(code == "P00001")
            {
                AddItem("Water Bottle", 1, "Mineral water. 33 cl", 1.20m);
                return true;
            }

            if(code == "P00002")
            {
                AddItem("Towel", 1, "Small soft towel", 4.00m);
                return true;
            }

            if(code == "P00003")
            {
                AddItem("Dumbbell", 1, "Small 3 kg dumbbell for exercise at home", 10.00m);
                return true;
            }

            return false;
        }

        public void Clear() { Items.Clear(); }


    }
}
