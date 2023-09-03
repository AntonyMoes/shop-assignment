using System;
using System.Collections.Generic;
using GeneralUtils;

namespace _Game.Scripts.Objects {
    public class Inventory {
        private readonly IInventoryObject[] _objects;
        public IList<IInventoryObject> Objects => _objects;

        private readonly Action _onInventoryUpdate;
        public readonly Event OnInventoryUpdate;

        public Inventory(int size) {
            OnInventoryUpdate = new Event(out _onInventoryUpdate);
            _objects = new IInventoryObject[size];
        }

        public bool AddObject(IInventoryObject inventoryObject, int? position = null) {
            if (position is not { } pos) {
                var emptyIndex = _objects.IndexOf(obj => obj == null);
                if (emptyIndex == -1) {
                    return false;
                }

                pos = emptyIndex;
            }

            if (_objects[pos] != null) {
                return false;
            }
 
            _objects[pos] = inventoryObject;
            _onInventoryUpdate();
            return true;
        }

        public void RemoveObject(IInventoryObject inventoryObject) {
            var index = _objects.IndexOf(inventoryObject);
            if (index != -1) {
                RemoveObject(index);
            }
        }

        public void RemoveObject(int position) {
            _objects[position] = null;
            _onInventoryUpdate();
        }
    }
}