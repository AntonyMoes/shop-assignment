using System;
using _Game.Scripts.Equipment;
using _Game.Scripts.Objects;
using GeneralUtils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class EquipmentPanel : UIElement {
        [SerializeField] private Button _closeButton;
        [SerializeField] private EquipmentPanelSlot[] _slots;

        private EquipmentController _equipmentController;
        private Action _onClose;
        private IInventoryObjectFactory _factory;

        protected override void Init() {
            _closeButton.onClick.AddListener(OnClose);
            foreach (var slot in _slots) {
                slot.OnEquipmentUpdate.Subscribe(equipment => OnEquipmentUpdate(slot, equipment));
            }
        }

        public void Setup(EquipmentController equipmentController, Action onClose, IInventoryObjectFactory inventoryObjectFactory) {
            _equipmentController = equipmentController;
            _onClose = onClose;
            _factory = inventoryObjectFactory;
        }

        protected override void PerformShow(Action onDone = null) {
            foreach (var slot in _slots) {
                var equipment = _equipmentController.Equipment.TryGetValue(slot.Slot, out var e) ? e as EquipmentObject : null;
                slot.Load(equipment, _factory);
            }
            
            base.PerformShow(onDone);
        }

        private void OnEquipmentUpdate(EquipmentPanelSlot equipmentPanelSlot, IEquipment equipment) {
            var slot = equipmentPanelSlot.Slot;
            if (equipment == null) {
                _equipmentController.RemoveEquipment(slot);
            } else {
                _equipmentController.SetEquipment(equipment);
            }
        }

        private void OnClose() {
            _onClose?.Invoke();
        }

        public override void Clear() {
            foreach (var slot in _slots) {
                slot.Clear();
            }
        }
    }
}