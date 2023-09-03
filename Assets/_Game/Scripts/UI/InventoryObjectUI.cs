using _Game.Scripts.Objects;
using _Game.Scripts.Shop;
using _Game.Scripts.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class InventoryObjectUI : MonoBehaviour {
        [SerializeField] private Image _image;
        [SerializeField] private DragComponent _dragComponent;
        [SerializeField] private GameObject _priceGroup;
        [SerializeField] private TextMeshProUGUI _price;

        public IInventoryObject InventoryObject { get; private set; }

        public void Init(CanvasScaler canvasScaler, Transform dragLayer, IInventoryObject inventoryObject,
            DropComponent initialSlot) {
            InventoryObject = inventoryObject;
            _image.sprite = InventoryObject.Sprite;
            _priceGroup.SetActive(false);

            _dragComponent.Init(canvasScaler, dragLayer, initialSlot);
        }

        public void SetShowPrice(bool show, float priceModifier) {
            _priceGroup.SetActive(show);
            _price.text = Price.ApplyModifier(InventoryObject.Price, priceModifier).ToString();
        }
    }
}