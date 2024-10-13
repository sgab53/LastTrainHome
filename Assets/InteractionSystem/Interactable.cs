using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace InteractionSystem
{
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private UnityEvent _onInteracted;
        [SerializeField] private UnityEvent _onSelected;
        [SerializeField] private UnityEvent _onDeselected;

        private float _priority, _priorityMod;
        public float Priority => _priority + _priorityMod;

        public UnityEvent OnInteractedEvent => _onInteracted;
        public UnityEvent OnSelectedEvent => _onSelected;
        public UnityEvent OnDeselectedEvent => _onDeselected;

        private void Awake()
        {
            _priority = _priorityMod = 0f;
        }

        public void Interact()
        {
            _onInteracted?.Invoke();
        }

        public void Select()
        {
            _onSelected?.Invoke();
        }

        public void Deselect()
        {
            _onDeselected?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _priorityMod = 9999f;
            Select();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _priorityMod = 0f;
            Deselect();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Interact();
                    break;
                case PointerEventData.InputButton.Right: // unused
                    break;
                case PointerEventData.InputButton.Middle: // unused
                    break;
                default:
                    break;
            }
        }
    }
}
