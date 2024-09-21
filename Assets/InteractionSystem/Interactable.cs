using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private bool _disableSelf;
    [SerializeField] private UnityEvent _onActivate;

    private Collider2D _trigger;

    private void OnValidate() {
        if (!_trigger)
            _trigger = (Collider2D)GetComponent(typeof(Collider2D));

        _trigger.isTrigger = true;
    }

    public void Activate()
    {
        Debug.Log("activate");
        if (_disableSelf)
            gameObject.SetActive(false);
        
        _onActivate?.Invoke();
    }
}