using Cysharp.Threading.Tasks;
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
        private InteractionService _interactionService;

        [Header("Interaction Events")]
        [SerializeField] private UnityEvent _onInteracted;
        [SerializeField] private UnityEvent _onSelected;
        [SerializeField] private UnityEvent _onDeselected;

        public UnityEvent OnInteractedEvent => _onInteracted;
        public UnityEvent OnSelectedEvent => _onSelected;
        public UnityEvent OnDeselectedEvent => _onDeselected;

        protected virtual void OnEnable()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            AddToInteractionService().Forget();
        }

        private async UniTaskVoid AddToInteractionService()
        {
            _interactionService = await Service.Get<InteractionService>();
            _interactionService.AddInteractable(gameObject, this);
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

        public virtual void Select()
        {
            _onSelected?.Invoke();
        }

        public virtual void Deselect()
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
