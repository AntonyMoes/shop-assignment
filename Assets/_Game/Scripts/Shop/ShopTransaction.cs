using System.Collections.Generic;
using _Game.Scripts.Objects;
using GeneralUtils;

namespace _Game.Scripts.Shop {
    public class ShopTransaction {
        private readonly Inventory _player;
        private readonly Inventory _shop;
        private readonly float _sellPriceModifier;
        private readonly Dictionary<IInventoryObject, bool> _transfers = new();

        private readonly UpdatedValue<int> _totalCost = new();
        public IUpdatedValue<int> TotalCost => _totalCost;

        public bool HasTransfers => _transfers.Count > 0;

        public ShopTransaction(Inventory player, Inventory shop, float sellPriceModifier) {
            _player = player;
            _shop = shop;
            _sellPriceModifier = sellPriceModifier;
        }

        public void AddTransfer(IInventoryObject obj, bool toPlayer) {
            if (_transfers.ContainsKey(obj)) {
                var existingToPlayer = _transfers[obj];
                if (existingToPlayer != toPlayer) {
                    _transfers.Remove(obj);
                    _totalCost.Value -= existingToPlayer ? obj.Price : -Price.ApplyModifier(obj.Price, _sellPriceModifier);
                }
            } else {
                _transfers.Add(obj, toPlayer);
                _totalCost.Value += toPlayer ? obj.Price : -Price.ApplyModifier(obj.Price, _sellPriceModifier);
            }
        }

        public void Rollback() {
            foreach (var (obj, toPlayer) in _transfers) {
                if (toPlayer) {
                    _player.RemoveObject(obj);
                    _shop.AddObject(obj);
                } else {
                    _shop.RemoveObject(obj);
                    _player.AddObject(obj);
                }
            }
        }
    }
}