using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public PlayerAnimationState CurrentState { get; private set; }

    private void Awake()
    {
        SetAnimation(PlayerAnimationState.IdleDown);
    }

    public void SetAnimation(PlayerAnimationState state)
    {
        CurrentState = state;
        _animator.Play(CurrentState.ToString());
    }
}

public enum PlayerAnimationState
{
    IdleUp, IdleDown, IdleLeft, IdleRight, WalkUp, WalkDown, WalkLeft, WalkRight, Seated
}