using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _minThreshold = 0.15f;

    [SerializeField] private Rigidbody2D _target;
    [SerializeField] private PlayerAnimationController _playerAnimController;

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
        
        if (_movement.y == 0)
        {
            if (_movement.x > 0) // right
            {
                _playerAnimController.SetAnimation(PlayerAnimationState.WalkRight);
                return;
            }
            else if (_movement.x < 0) // left
            {
                _playerAnimController.SetAnimation(PlayerAnimationState.WalkLeft);
                return;
            }
        }
        else if (_movement.x == 0)
        {
            if (_movement.y > 0) // up
            {
                _playerAnimController.SetAnimation(PlayerAnimationState.WalkUp);
                return;
            }
            else if (_movement.y < 0) // down
            {
                _playerAnimController.SetAnimation(PlayerAnimationState.WalkDown);
                return;
            }
        }
        else if (Mathf.Abs(_movement.x) - Mathf.Abs(_movement.y) < _minThreshold)
        {
            return;
        }
        
        if (_playerAnimController.CurrentState == PlayerAnimationState.Seated ||
        (int)_playerAnimController.CurrentState < 4)
            return;

        var newState = _playerAnimController.CurrentState - 4;
        Debug.Log(newState);
        _playerAnimController.SetAnimation(newState);
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