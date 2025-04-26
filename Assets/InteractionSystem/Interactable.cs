using LTH.Core.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LTH.InteractionSystem
{
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private UnityEvent _onInteracted;
        [SerializeField] private UnityEvent _onSelected;
        [SerializeField] private UnityEvent _onDeselected;

        private InteractionService _interactionService;

        public UnityEvent OnInteractedEvent => _onInteracted;
        public UnityEvent OnSelectedEvent => _onSelected;
        public UnityEvent OnDeselectedEvent => _onDeselected;

        private void OnEnable()
        {
            if (!_interactionService)
                _interactionService = ServiceLocator.Instance.GetService<InteractionService>();

            _interactionService.AddInteractable(gameObject, this);
        }

        private void OnDisable()
        {
            if (!_interactionService)
                return;

            _interactionService.RemoveInteractable(gameObject);
        }

        private void OnDestroy()
        {
            _interactionService = null;
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
            _interactionService.ForceTarget(this);
            _interactionService.SwapTargetIfValid(gameObject);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _interactionService.UnsetForcedTarget();
            _interactionService.UnsetTarget();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    _interactionService.InteractWithSelectedTarget();
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
