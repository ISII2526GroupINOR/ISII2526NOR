using AppForSEII2526.Web.API;
using NuGet.Configuration;

namespace AppForSEII2526.Web
{
    public class RestockStateContainer
    {
        public RestockForCreateDTO Restock { get; private set; } = new RestockForCreateDTO()
        {
            RestockItems = new List<ItemForCreateRestockDTO>()
        };

        public event Action? OnChange;

        private void NotifyStateHasChanged() => OnChange?.Invoke();

        public void AddItemToRestock(ItemForRestockingDTO item, int restockQuantity)
        {
            if(!Restock.RestockItems.Any(ri => ri.Id == item.Id))
            {
                Restock.RestockItems.Add(new ItemForCreateRestockDTO { Id = item.Id, RestockQuantity = restockQuantity });
            }
        }

        public void RemoveRestockItemToRestock(ItemForCreateRestockDTO item)
        {
            Restock.RestockItems.Remove(item);
        }

        public void ClearRestockCar()
        {
            Restock.RestockItems.Clear();
        }

        public void RestockProcessed()
        {
            Restock = new RestockForCreateDTO()
            {
                RestockItems = new List<ItemForCreateRestockDTO>()
            };
        }
    }
}
