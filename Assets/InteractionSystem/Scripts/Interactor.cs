using Cysharp.Threading.Tasks;
using LTH.Core.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LTH.InteractionSystem
{
    public sealed class Interactor : MonoBehaviour
    {
        [SerializeField] private InputActionReference _interact;
        private InteractionService _interactionService;

        [Header("Parameters")]
        [SerializeField] private float _interactionRadius = 3f;
        [SerializeField] private LayerMask interactableLayers;

        private readonly Collider[] _hits = new Collider[8];

        private void OnEnable()
        {
            _interact.action.performed += OnInteractActionPerformed;
        }

        private void OnDisable()
        {
            _interact.action.performed -= OnInteractActionPerformed;
        }

        private void Awake()
        {
            InitInteractor().Forget();
        }

        private async UniTaskVoid InitInteractor()
        {
            _interactionService = await Service.Get<InteractionService>();
        }

        private void OnInteractActionPerformed(InputAction.CallbackContext _)
        {
            _interactionService.InteractWithSelectedTarget();
        }

        private void Update()
        {
            if (!_interactionService)
                return;

            var count = Physics.OverlapSphereNonAlloc(transform.position, _interactionRadius, _hits,
                interactableLayers);

            if (count <= 0)
            {
                _interactionService.UnsetTarget();
                return;
            }

            var nearest = FindNearest();

            if (!nearest)
            {
                _interactionService.UnsetTarget();
                return;
            }

            _interactionService.SwapTargetIfValid(nearest.gameObject);
            return;

            Collider FindNearest()
            {
                nearest = null;
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
}