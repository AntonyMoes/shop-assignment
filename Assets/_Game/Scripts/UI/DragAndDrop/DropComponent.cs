using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.UI.DragAndDrop {
    public class DropComponent : MonoBehaviour, IDropHandler {
        [SerializeField] private bool _setDragPosition = true;
        [SerializeField] private Transform _dragParent;

        private Func<DragComponent, bool> _canDrop;

        private readonly Action<DragComponent> _onDropped;
        public readonly GeneralUtils.Event<DragComponent> OnDropped;

        public DragComponent DragComponent { get; private set; }

        public DropComponent() {
            OnDropped = new GeneralUtils.Event<DragComponent>(out _onDropped);
        }

        public void Init(Func<DragComponent, bool> canDrop = null) {
            _canDrop = canDrop;
        }

        public void OnDrop(PointerEventData eventData) {
            var dragObject = eventData.pointerDrag;
            if (dragObject == null || !dragObject.TryGetComponent<DragComponent>(out var dragComponent)) {
                return;
            }


            var canDrop = (_canDrop?.Invoke(dragComponent) ?? true) &&
                          (dragComponent.DropComponent == null || DragComponent == null ||
                           (dragComponent.DropComponent._canDrop?.Invoke(DragComponent) ?? true));

            dragComponent.EndDrag(canDrop, this);
        }

        public void Drop(DragComponent dragComponent) {
            if (_setDragPosition && dragComponent != null) {
                var dragTransform = dragComponent.transform;
                dragTransform.SetParent(_dragParent != null ? _dragParent : transform);
                dragTransform.localPosition = Vector3.zero;
            }

            DragComponent = dragComponent;
            _onDropped(dragComponent);
        }

        public void SetInitialDrag(DragComponent dragComponent) {
            DragComponent = dragComponent;
        }
    }
}