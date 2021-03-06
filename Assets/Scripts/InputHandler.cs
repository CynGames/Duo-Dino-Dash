using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Command _jump, _crouch, _special, _moveLeft, _moveRight, _die, _run;
    private Animator _animator;
    private Rigidbody _rigidbody;
    
    public enum TouchDirection { UP, LEFT, RIGHT, DOWN, TAP }
    [SerializeField] private TouchDirection direction;
    [SerializeField] [Range(0, 5)] private float tapThreshold = 2;
    [SerializeField] [Range(0, 1)] private float animationCancelThreshold = 0.1f;
    private Vector2 _touchInitialPosition, _touchFinalPosition;
    private Touch _touch;

    private void Start()
    {
        InitializeComponents();
        InitializeCommands();
    }
    
    private void InitializeComponents()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void InitializeCommands()
    {
        _die = new DoDie();
        _run = new DoRun();
        _jump = new DoJump();
        _crouch = new DoCrouch();
        _special = new DoSpecial();
        _moveLeft = new DoMoveLeft();
        _moveRight = new DoMoveRight();
    }
    
    private void Update() => RunUpdate();

    private void RunUpdate()
    {
        if (Input.touchCount > 0) HandleTouch();
    }

    private void HandleTouch()
    {
        _touch = Input.GetTouch(0);

        UpdateTouchPosition();

        if (_touch.phase == TouchPhase.Ended) ExecuteAction();
    }
    
    private void UpdateTouchPosition()
    {
        switch (_touch)
        {
            case {phase: TouchPhase.Began}:
                _touchInitialPosition = _touch.position;
                break;
            case {phase: TouchPhase.Ended}:
                _touchFinalPosition = _touch.position;
                break;
        }
    }

    private void ExecuteAction()
    {
        direction = GetTouchDirection();

        if (ShouldExecuteCommand() == false) return;

        // Para debug
        GameManager.Instance.WriteOnScreen(direction);

        ExecuteCommand();
    }

    private TouchDirection GetTouchDirection()
    {
        var (x, y) = GetTouchPositionDifference();
        
        if (Mathf.Abs(x) <= tapThreshold && Mathf.Abs(y) <= tapThreshold)
            return TouchDirection.TAP;
        
        if (Mathf.Abs(x) > Mathf.Abs(y)) 
            return x > 0 ? TouchDirection.RIGHT : TouchDirection.LEFT;
        
        return y > 0 ? TouchDirection.UP : TouchDirection.DOWN;
    }
    
    private (float x, float y) GetTouchPositionDifference()
    {
        var x = _touchFinalPosition.x - _touchInitialPosition.x;
        var y = _touchFinalPosition.y - _touchInitialPosition.y;
        return (x, y);
    }
    
    private bool ShouldExecuteCommand() =>
        CommandCanBePerformedAnytime() || AnimationPlayedLongEnough(GetAnimationDuration());

    private bool CommandCanBePerformedAnytime() => direction == TouchDirection.LEFT || direction == TouchDirection.RIGHT;
    
    private float GetAnimationDuration() => !IsPlayingRunAnimation() ? GetCurrentAnimationInfo().length: 0;

    private bool IsPlayingRunAnimation() => GetCurrentAnimationInfo().IsName("run");

    private AnimatorStateInfo GetCurrentAnimationInfo(int index = 0) => _animator.GetCurrentAnimatorStateInfo(index);

    private bool AnimationPlayedLongEnough(float animationDuration) => animationDuration < animationCancelThreshold;
    
    private void ExecuteCommand()
    {
        switch (direction)
        {
            case TouchDirection.UP:
                _jump.Execute(_animator, _rigidbody);
                break;
            case TouchDirection.LEFT:
                _moveLeft.Execute(_animator, _rigidbody);
                break;
            case TouchDirection.RIGHT:
                _moveRight.Execute(_animator, _rigidbody);
                break;
            case TouchDirection.DOWN:
                _crouch.Execute(_animator, _rigidbody);
                break;
            case TouchDirection.TAP:
                _special.Execute(_animator, _rigidbody);
                break;
        }
    }
}