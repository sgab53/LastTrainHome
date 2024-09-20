using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _minThreshold = 0.15f;

    [SerializeField] private Rigidbody2D _target;

    private Vector2 _movement;

    private void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        _movement = Vector2.zero;

        if (Mathf.Abs(horizontal) > _minThreshold)
        {
            _movement.x = horizontal * _speed;
            _movement.y = 0f;
        }
        else if (Mathf.Abs(vertical) > _minThreshold)
        {
            _movement.x = 0f;
            _movement.y = vertical * _speed;
        }
    }

    private void FixedUpdate()
    {
        _target.velocity = _movement;
    }
}