using System;
using _Game.Scripts.Player;
using GeneralUtils;
using GeneralUtils.UI;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class UIManager : MonoBehaviour, IInteractPanelPresenter, IInputBlocker {
        [SerializeField] private Camera _uiCamera;

        [SerializeField] private InteractPanel _interactPanel;

        private int _activeBlockingElements;
        private readonly UpdatedValue<bool> _inputBlocked = new();
        public IUpdatedValue<bool> InputBlocked => _inputBlocked;

        public void Init(Camera gameCamera) {
            _interactPanel.Setup(_uiCamera, gameCamera);

            // TODO
            var blockingElements = new UIElement[] {};
            foreach (var blockingElement in blockingElements) {
                blockingElement.State.Subscribe(OnBlockingElementStateChanged);
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
    }
}