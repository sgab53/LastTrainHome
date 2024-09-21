using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent _onActivate;

    public void Activate()
    {
        _onActivate?.Invoke();
    }
}