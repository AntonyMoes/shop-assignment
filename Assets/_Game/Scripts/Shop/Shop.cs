using System.Linq;
using _Game.Scripts.Objects;

namespace _Game.Scripts.Shop {
    public class Shop {
        public readonly float SellPriceModifier;
        public readonly Inventory Inventory;

        public Shop(float sellPriceModifier, int inventorySize, string[] goods, InventoryObjectProvider provider) {
            SellPriceModifier = sellPriceModifier;
            Inventory = new Inventory(inventorySize);
            foreach (var obj in goods.Select(provider.GetObject)) {
                Inventory.AddObject(obj);
            }
        }
    }
}