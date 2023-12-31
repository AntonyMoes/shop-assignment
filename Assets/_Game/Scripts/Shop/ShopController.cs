﻿using System.Linq;
using _Game.Scripts.Interaction;
using _Game.Scripts.Objects;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.Shop {
    public class ShopController : Interactable {
        [SerializeField] private float _sellPriceModifier;
        [SerializeField] private string[] _goods;

        private const int ShopInventorySize = 9;

        private IShopPanelPresenter _shopPanelPresenter;
        private Shop _shop;

        public void Setup(IShopPanelPresenter shopPanelPresenter, InventoryObjectProvider provider) {
            _shopPanelPresenter = shopPanelPresenter;
            _shop = new Shop(_sellPriceModifier, ShopInventorySize, _goods, provider);
        }

        public override void Interact() {
            _shopPanelPresenter.ShowShopPanel(_shop.SellPriceModifier, _shop.Inventory);
        }
    }
}