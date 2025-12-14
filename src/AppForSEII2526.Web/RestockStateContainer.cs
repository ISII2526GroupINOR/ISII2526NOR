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

        public Dictionary<int, ItemForRestockingDTO> itemInfo { get; private set; } = new Dictionary<int, ItemForRestockingDTO>();

        public event Action? OnChange;

        private void NotifyStateHasChanged() => OnChange?.Invoke();

        public void AddItemToRestock(ItemForRestockingDTO item, int restockQuantity)
        {
            if(!Restock.RestockItems.Any(ri => ri.Id == item.Id))
            {
                itemInfo[item.Id] = item; 
                Restock.RestockItems.Add(new ItemForCreateRestockDTO { Id = item.Id, RestockQuantity = restockQuantity });
            }
        }

        public void RemoveRestockItemToRestock(ItemForCreateRestockDTO item)
        {
            itemInfo.Remove(item.Id);
            Restock.RestockItems.Remove(item);
        }

        public void ClearRestockCar()
        {
            itemInfo.Clear();
            Restock.RestockItems.Clear();
        }

        public void RestockProcessed()
        {
            itemInfo = new Dictionary<int, ItemForRestockingDTO>();
            Restock = new RestockForCreateDTO()
            {
                RestockItems = new List<ItemForCreateRestockDTO>()
            };
        }
    }
}
