﻿using _Game.Scripts.Interaction;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts {
    public class Map : MonoBehaviour {
        [SerializeField] private Interactable[] _interactables;

        public void Init(IInteractPanelPresenter interactPanelPresenter) {
            foreach (var interactable in _interactables) {
                interactable.Init(interactPanelPresenter);
            }
        }
    }
}