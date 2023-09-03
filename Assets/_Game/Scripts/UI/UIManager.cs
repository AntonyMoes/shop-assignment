using System;
using _Game.Scripts.Equipment;
using _Game.Scripts.Objects;
using _Game.Scripts.Player;
using _Game.Scripts.UI.DragAndDrop;
using GeneralUtils;
using GeneralUtils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class UIManager : MonoBehaviour, IInputBlocker, IInteractPanelPresenter, IEquipmentPanelPresenter, IShopPanelPresenter, IInventoryObjectFactory {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Transform _dragLayer;
        [SerializeField] private CanvasScaler _canvasScaler;

        [SerializeField] private InventoryObjectUI _inventoryObjectPrefab;

        [SerializeField] private InteractPanel _interactPanel;
        [SerializeField] private InventoryPanel _inventoryPanel;
        [SerializeField] private EquipmentPanel _equipmentPanel;
        [SerializeField] private ShopPanel _shopPanel;

        private int _activeBlockingElements;
        private readonly UpdatedValue<bool> _inputBlocked = new();
        public IUpdatedValue<bool> InputBlocked => _inputBlocked;

        public void Init(Camera gameCamera) {
            _interactPanel.Setup(_uiCamera, gameCamera);

            var blockingElements = new UIElement[] {
                _equipmentPanel,
                _shopPanel
            };
            foreach (var blockingElement in blockingElements) {
                blockingElement.State.Subscribe(OnBlockingElementStateChanged);
            }
        }

        public void InitPlayer(EquipmentController equipmentController, Inventory inventory) {
            _equipmentPanel.Setup(equipmentController, HideEquipmentPanel, this);
            _inventoryPanel.Setup(inventory, this);
            _shopPanel.Setup(inventory, HideShopPanel, this);
        }

        private void OnBlockingElementStateChanged(UIElement.EState state) {
            switch (state) {
                case UIElement.EState.Shown:
                    if (_activeBlockingElements == 0) {
                        _inputBlocked.Value = true;
                    }

                    _activeBlockingElements++;
                    break;
                case UIElement.EState.Hided:
                    _activeBlockingElements--;

                    if (_activeBlockingElements == 0) {
                        _inputBlocked.Value = false;
                    }
                    break;
            }
        }

        public void ShowInteractPanel(Transform anchor) {
            _interactPanel.Load(anchor);
            _interactPanel.Show();
        }

        public void HideInteractPanel(Transform anchor) {
            if (_interactPanel.Anchor == anchor) {
                _interactPanel.Hide();
            }
        }

        public void ShowEquipmentPanel() {
            _inventoryPanel.Load(false);
            _inventoryPanel.Show();
            _equipmentPanel.Show();
        }

        public void HideEquipmentPanel() {
            _inventoryPanel.Hide();
            _equipmentPanel.Hide();
        }

        public void ShowShopPanel(float sellPriceModifier, Inventory shopInventory) {
            _inventoryPanel.Load(true, sellPriceModifier);
            _inventoryPanel.Show();
            _shopPanel.Load(sellPriceModifier, shopInventory);
            _shopPanel.Show();
        }

        public void HideShopPanel() {
            _inventoryPanel.Hide();
            _shopPanel.Hide();
        }

        public InventoryObjectUI CreateInventoryObject(Transform parent, IInventoryObject inventoryObject, DropComponent initialSlot) {
            var inventoryObjectUI = Instantiate(_inventoryObjectPrefab, parent);
            inventoryObjectUI.Init(_canvasScaler, _dragLayer, inventoryObject, initialSlot);
            return inventoryObjectUI;
        }
    }
}