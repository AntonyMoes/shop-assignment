using UnityEngine;

namespace _Game.Scripts.Objects {
    public interface IInventoryObject {
        public Sprite Sprite { get; }
        public int Price { get; }
    }
}