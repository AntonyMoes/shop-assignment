using _Game.Scripts.Objects;
using _Game.Scripts.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class InventoryObjectUI : MonoBehaviour {
        [SerializeField] private Image _image;
        [SerializeField] private DragComponent _dragComponent;

        public IInventoryObject InventoryObject { get; private set; }

        public void Init(CanvasScaler canvasScaler, Transform dragLayer, IInventoryObject inventoryObject, DropComponent initialSlot) {
            InventoryObject = inventoryObject;
            _image.sprite = InventoryObject.Sprite;

            _dragComponent.Init(canvasScaler, dragLayer, initialSlot);
        }
    }
}