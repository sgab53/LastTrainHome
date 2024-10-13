using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private InputActionReference _interact;
        [SerializeField] private float _interactionRadius = 3f;
        [SerializeField] private LayerMask interactableLayers;

        private readonly Collider[] _hits = new Collider[8];
        private Interactable _target;

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
            if (!_target)
                return;

            _target.Interact();
        }

        private void Update()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, _interactionRadius, _hits,
                interactableLayers);

            if (count <= 0)
            {
                SwapTarget(null);
                return;
            }

            var nearest = FindNearest(count);

            if (!nearest)
            {
                SwapTarget(null);
                return;
            }

            if (!_target)
            {
                SwapTarget((Interactable)nearest.GetComponent(typeof(Interactable)));
                return;
            }

            if (_target.gameObject == nearest.gameObject)
                return;

            SwapTarget((Interactable)nearest.GetComponent(typeof(Interactable)));
        }

        private Collider FindNearest(int count)
        {
            Collider nearest = null;
            var closestDist = Mathf.Infinity;

            for (var i = 0; i < count; ++i)
            {
                var c = _hits[i];
                var dist = (c.transform.position - transform.position).sqrMagnitude;

                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearest = c;
                }
            }

            return nearest;
        }

        private void SwapTarget(Interactable newTarget)
        {
            _target?.Deselect();
            _target = newTarget;
            _target?.Select();
        }
    }
}