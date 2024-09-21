using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private GameStateData _gameState;
    [SerializeField] private DialoguePlayer _dialoguePlayer;
    
    [Header("Components")]
    [SerializeField] private CircleCollider2D _sensor;
    [SerializeField] private float _activationRange = 2f;

    private Interactable _cachedInteractable;

    private void OnValidate()
    {
        if (!_sensor)
            _sensor = (CircleCollider2D)GetComponent(typeof(CircleCollider2D));

        _sensor.radius = _activationRange;
        _sensor.isTrigger = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            Debug.Log("premuto");
            switch (_gameState.InputContext)
            {
                case InputContext.Game:
                    if (_cachedInteractable)
                    {
                        _cachedInteractable.Activate();
                        _cachedInteractable = null;
                    }
                    break;
                case InputContext.Dialogue:
                    _dialoguePlayer.Next();
                    break;
                default:
                    break;
            }
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
        if (!_sensor)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensor.transform.position, _activationRange);
    }
}
