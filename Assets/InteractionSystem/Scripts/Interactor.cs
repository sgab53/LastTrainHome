using LTH.Core.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LTH.InteractionSystem
{
    public sealed class Interactor : MonoBehaviour
    {
        [SerializeField] private InputActionReference _interact;

        [Header("Parameters")]
        [SerializeField] private float _interactionRadius = 3f;
        [SerializeField] private LayerMask interactableLayers;

        [Header("Optional")]
        [SerializeField] private InteractionService _interactionService;

        private readonly Collider[] _hits = new Collider[8];

        private void Awake()
        {
            if (!_interactionService)
                _interactionService = Service.Load<InteractionService>();
        }

        private void OnEnable()
        {
            _interact.action.performed += OnInteractActionPerformed;
        }

        private void OnDisable()
        {
            _interact.action.performed -= OnInteractActionPerformed;
        }

        private void OnInteractActionPerformed(InputAction.CallbackContext _)
        {
            _interactionService.InteractWithSelectedTarget();
        }

        private void Update()
        {
            if (!_interactionService)
                return;

            var position = transform.position;
            var count = Physics.OverlapSphereNonAlloc(position, _interactionRadius, _hits,
                interactableLayers);

            if (count <= 0)
            {
                _interactionService.UnsetTarget();
                return;
            }

            var nearest = FindNearest(_hits, count, position);

            if (nearest < 0)
            {
                _interactionService.UnsetTarget();
                return;
            }

            _interactionService.SwapTargetIfValid(_hits[nearest].gameObject);
            return;

            static int FindNearest(Collider[] hits, int count, Vector3 origin)
            {
                var nearest = -1;
                var closestDist = Mathf.Infinity;

                for (var i = 0; i < count; ++i)
                {
                    var colliderPos = hits[i].transform.position;
                    var dist = (colliderPos - origin).sqrMagnitude;

                    if (dist >= closestDist)
                        continue;

                    closestDist = dist;
                    nearest = i;
                }

                return nearest;
            }
        }
    }
}