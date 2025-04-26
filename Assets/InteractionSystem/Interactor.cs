using LTH.Core.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LTH.InteractionSystem
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private InputActionReference _interact;

        [Header("Parameters")]
        [SerializeField] private float _interactionRadius = 3f;
        [SerializeField] private LayerMask interactableLayers;

        private InteractionService _interactionService;

        private readonly Collider[] _hits = new Collider[8];

        private void OnEnable()
        {
            _interact.action.performed += OnInteractActionPerformed;
        }

        private void OnDisable()
        {
            _interact.action.performed -= OnInteractActionPerformed;
        }

        private void Start()
        {
            _interactionService = ServiceLocator.Instance.GetService<InteractionService>();
        }

        private void OnInteractActionPerformed(InputAction.CallbackContext _)
        {
            _interactionService.InteractWithSelectedTarget();
        }

        private void Update()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, _interactionRadius, _hits,
                interactableLayers);

            if (count <= 0)
            {
                _interactionService.UnsetTarget();
                return;
            }

            var nearest = FindNearest(count);

            if (!nearest)
            {
                _interactionService.UnsetTarget();
                return;
            }

            _interactionService.SwapTargetIfValid(nearest.gameObject);
        }

        private Collider FindNearest(int count)
        {
            Collider nearest = null;
            var closestDist = Mathf.Infinity;

            for (var i = 0; i < count; ++i)
            {
                var c = _hits[i];
                var dist = (c.transform.position - transform.position).sqrMagnitude;

                if (dist >= closestDist)
                    continue;

                closestDist = dist;
                nearest = c;
            }

            return nearest;
        }
    }
}