using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.UI.DragAndDrop {
    public class DragComponent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
        [SerializeField] private bool _canDropAnywhere;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        private CanvasScaler _canvasScaler;
        private Transform _dragLayer;

        public DropComponent DropComponent { get; private set; }

        private bool _dragging;
        private Vector2 _initialAnchoredPosition;
        private Transform _initialParent;
        private float _initialAlpha;

        private const float AlphaScaling = 0.8f;

        public void Init(CanvasScaler canvasScaler, Transform dragLayer, DropComponent dropComponent = null) {
            _canvasScaler = canvasScaler;
            _dragLayer = dragLayer;
            DropComponent = dropComponent;
            if (dropComponent != null) {
                dropComponent.SetInitialDrag(this);
            }
        }

        public void OnDrag(PointerEventData eventData) {
            _rectTransform.anchoredPosition += eventData.delta / _canvasScaler.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _dragging = true;

            _initialAnchoredPosition = _rectTransform.anchoredPosition;
            _initialParent = _rectTransform.parent;
            _rectTransform.SetParent(_dragLayer, true);

            _canvasGroup.blocksRaycasts = false;
            _initialAlpha = _canvasGroup.alpha;
            _canvasGroup.alpha = _initialAlpha * AlphaScaling;
        }

        public void OnEndDrag(PointerEventData eventData) {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = _initialAlpha;

            var currentObject = eventData.pointerCurrentRaycast.gameObject;
            var endOnDrop = currentObject != null && currentObject.TryGetComponent<DropComponent>(out _);
            if (_dragging && !endOnDrop) {
                EndDrag(_canDropAnywhere, null);
            }
        }

        public void EndDrag(bool success, DropComponent dropComponent) {
            _dragging = false;

            if (!success) {
                _rectTransform.SetParent(_initialParent);
                _rectTransform.anchoredPosition = _initialAnchoredPosition;
            } else {
                if (DropComponent != null) {
                    var otherDrag = dropComponent != null ? dropComponent.DragComponent : null;
                    if (otherDrag != null) {
                        otherDrag.DropComponent = DropComponent;
                    }

                    DropComponent.Drop(otherDrag);
                }

                DropComponent = dropComponent;

                if (DropComponent != null) {
                    DropComponent.Drop(this);
                }
            }
        }
    }
}