using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Objects {
    public class InventoryObjectProvider : MonoBehaviour {
        [SerializeField] private SerializedInventoryObject[] _serializedInventoryObjects;

        public IInventoryObject GetObject(string objectName) {
            return _serializedInventoryObjects
                .FirstOrDefault(obj => obj.Name == objectName)
                ?.CreateInventoryObject();
        }
    }
}