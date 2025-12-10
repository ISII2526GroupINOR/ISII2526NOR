using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO()
        {
            Items = new List<ItemForPurchaseCreateDTO>()
        };

        public decimal TotalPrice
        {
            get{
                return 0; 
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddSelectedItemToPurchase(ItemForPurchaseSelectDTO itemForPurchase)
        {
            if (!Purchase.Items.Any(i => i.Id == itemForPurchase.Id))
            {
                Purchase.Items.Add(new ItemForPurchaseCreateDTO()
                {
                    Id = itemForPurchase.Id,
                    Name = itemForPurchase.Name,
                    Description = itemForPurchase.Description,
                    Brand = itemForPurchase.Brand,
                    PurchasePrice = itemForPurchase.PurchasePrice
                });
            }
        }

        public void RemoveSelectedItemFromPurchase(ItemForPurchaseCreateDTO itemForPurchase)
        {
            Purchase.Items.Remove(
                itemForPurchase
                );
        }

        public void ClearSelectedItemsInPurchase()
        {
            Purchase.Items.Clear();
        }

        public void PurchaseCreationProcessed()
        {
            Purchase = new PurchaseForCreateDTO()
            {
                Items = new List<ItemForPurchaseCreateDTO>()
            };
        }
    }
}
