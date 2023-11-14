using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float Speed;
    [SerializeField] private float _speedMultiplyer;
    private Vector3 _velocity;

    [Header("Jump")]
    [SerializeField] public float JumpHeight;
    [SerializeField] private float _jumpHeightMultiplyer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] public bool IsGrounded = true;
    [SerializeField] private AudioClip[] _stepSounds;
    [SerializeField] private AudioClip[] _jumpSounds;
    [SerializeField] private AudioClip[] _landSounds;

    [Header("References")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;

    private InputAction MoveInputAction;
    private InputAction JumpInputAction;
    public Vector3 _walkInput;


    private void OnEnable()
    {
        MoveInputAction = _playerInput.currentActionMap.FindAction("Move");
        JumpInputAction = _playerInput.currentActionMap.FindAction("Jump");
        MoveInputAction.started += WalkInputStart;
        MoveInputAction.performed += WalkInput;
        MoveInputAction.canceled += WalkInputStop;
        JumpInputAction.performed += JumpInput;
    }

    
    private void WalkInputStart(InputAction.CallbackContext obj) => _animator.SetBool("IsRunning", true);
    private void WalkInput(InputAction.CallbackContext obj)
    {
        _walkInput = obj.ReadValue<Vector2>();
    }

    private void WalkInputStop(InputAction.CallbackContext obj)
    {
        _animator.SetBool("IsRunning", false);
        _walkInput = default;
    }

    private void JumpInput(InputAction.CallbackContext obj)
    {
        if (IsGrounded) _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
    }

    private void OnDisable()
    {
        MoveInputAction.started -= WalkInputStart;
        MoveInputAction.performed -= WalkInput;
        MoveInputAction.canceled -= WalkInputStop;
        JumpInputAction.performed -= JumpInput;
    }

    private void Start()
    {
        IsGrounded = true;
        Speed *= _speedMultiplyer;
        JumpHeight *= _jumpHeightMultiplyer;
        _animator.SetFloat("SpeedMultyplyer", _speedMultiplyer);
    }

    private void Update()
    {
        Move();
        CheckIfGrounded();
        Fall();
        ApplyVerticalVelocity();
    }

    // Called from Animator event
    public void MakeStepSound()
    {
        SoundManager.Instance.PlaySoundRandom(_stepSounds);
    }

    private void Move()
    {
        if (_walkInput == default) return;
        Vector3 movement = transform.right * _walkInput.x + transform.forward * _walkInput.y;
        _characterController.Move(movement * Speed * Time.deltaTime);
    }

    private void CheckIfGrounded()
    {
        if (!IsGrounded)
        {
            IsGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _whatIsGround);
            if (IsGrounded) SoundManager.Instance.PlaySoundRandom(_landSounds);
        }
        else
        {
            IsGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _whatIsGround);
            if (!IsGrounded) SoundManager.Instance.PlaySoundRandom(_jumpSounds);
        }
    }

    private void Fall()
    {
        if (!_characterController.isGrounded && !IsGrounded)
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        if (_characterController.isGrounded && IsGrounded && _velocity.y < 0)
        {
            _velocity.y = 0;
        }     
    }

    private void ApplyVerticalVelocity()
    {
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
