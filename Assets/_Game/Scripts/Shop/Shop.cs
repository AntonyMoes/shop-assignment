using System.Linq;
using _Game.Scripts.Interaction;
using _Game.Scripts.Objects;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.Shop {
    public class Shop : Interactable {
        [SerializeField] private float _sellPriceModifier;
        [SerializeField] private string[] _goods;

        private const int ShopInventorySize = 9;

        private IShopPanelPresenter _shopPanelPresenter;
        private Inventory _shopInventory;

        public void Setup(IShopPanelPresenter shopPanelPresenter, InventoryObjectProvider provider) {
            _shopPanelPresenter = shopPanelPresenter;

            _shopInventory = new Inventory(ShopInventorySize);
            foreach (var obj in _goods.Select(provider.GetObject)) {
                _shopInventory.AddObject(obj);
            }
        }

        public override void Interact() {
            _shopPanelPresenter.ShowShopPanel(_sellPriceModifier, _shopInventory);
        }
    }
}