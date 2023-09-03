using UnityEngine;

namespace _Game.Scripts.Objects {
    public abstract class SerializedInventoryObject : ScriptableObject {
        public abstract string Name { get; }
        public abstract IInventoryObject CreateInventoryObject();
    }
}