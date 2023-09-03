using _Game.Scripts.Objects;

namespace _Game.Scripts.UI {
    public interface IShopPanelPresenter {
        public void ShowShopPanel(float sellPriceModifier, Inventory shopInventory);
        public void HideShopPanel();
    }
}