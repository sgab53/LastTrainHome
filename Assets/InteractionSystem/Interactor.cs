using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float _activationRange = 2f;

    private Interactable _cachedInteractable;

    private void Update()
    {
        if (Input.GetButton("Action"))
        {
            _cachedInteractable.Activate();
            _cachedInteractable = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_cachedInteractable != null)
            return;

        _cachedInteractable = (Interactable)other.GetComponent(typeof(Interactable));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = (Interactable)other.GetComponent(typeof(Interactable));
        
        if (_cachedInteractable != interactable)
            return;
        
        _cachedInteractable = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _activationRange);
    }
}
