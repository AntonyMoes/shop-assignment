using _Game.Scripts.Objects;
using _Game.Scripts.UI.DragAndDrop;
using UnityEngine;

namespace _Game.Scripts.UI {
    public interface IInventoryObjectFactory {
        public InventoryObjectUI CreateInventoryObject(Transform parent, IInventoryObject inventoryObject, DropComponent initialSlot);
    }
}