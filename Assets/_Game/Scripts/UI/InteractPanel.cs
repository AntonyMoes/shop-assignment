using System;
using GeneralUtils.UI;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class InteractPanel : UIElement {
        [SerializeField] private RectTransform _rectTransform;

        private Transform _anchor;
        private Camera _uiCamera;
        private Camera _gameCamera;

        public Transform Anchor => _anchor;

        public void Setup(Camera uiCamera, Camera gameCamera) {
            _uiCamera = uiCamera;
            _gameCamera = gameCamera;
        }

        public void Load(Transform anchor) {
            _anchor = anchor;
        }

        private void Update() {
            if (_anchor == null) {
                return;
            }

            var viewportPoint = _gameCamera.WorldToViewportPoint(_anchor.position);
            var screenPoint = _uiCamera.ViewportToScreenPoint(viewportPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) transform.parent, screenPoint,
                _uiCamera, out var localPoint);
            _rectTransform.anchoredPosition = localPoint;
        }
    }
}