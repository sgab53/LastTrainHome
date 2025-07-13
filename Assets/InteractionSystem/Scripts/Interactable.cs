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

        [Header("Optional")]
        [SerializeField] private InteractionService _interactionService;

        public UnityEvent OnInteractedEvent => _onInteracted;
        public UnityEvent OnSelectedEvent => _onSelected;
        public UnityEvent OnDeselectedEvent => _onDeselected;

        protected virtual void Awake()
        {
            if (!_interactionService)
                _interactionService = Service.Load<InteractionService>();
        }

        protected virtual void OnEnable()
        {
            _interactionService.AddInteractable(gameObject, this);
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        protected virtual void OnDisable()
        {
            _interactionService.RemoveInteractable(gameObject);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        public virtual void Interact()
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
            if (eventData.button == PointerEventData.InputButton.Left)
                _interactionService.InteractWithSelectedTarget();
        }
    }
}
