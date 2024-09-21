using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _minThreshold = 0.15f;

    [SerializeField] private Rigidbody2D _target;

    private float _threshold = 0f;
    private Vector2 _movement = new();

    private void OnValidate()
    {
        _threshold = _minThreshold * _minThreshold;
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (_movement.sqrMagnitude > _threshold)
        {
            _movement.Normalize();
            _target.velocity = _movement * _speed;
        }
        else
        {
            _target.velocity = Vector2.zero;
        }
    }
}