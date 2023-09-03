using UnityEngine;

namespace _Game.Scripts.Interaction {
    public class TestInteractable : Interactable {
        public override void Interact() {
            Debug.Log("Interact!");
        }
    }
}