using Input;
using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float Speed;
    [SerializeField] private float _speedMultiplyer;
    public Vector3 MoveInput;
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
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;

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
        ToggleAnimationState();
    }

    // Called from Animator event
    public void MakeStepSound()
    {
        SoundManager.Instance.PlaySoundRandom(_stepSounds);
    }

    private void Move()
    {
        if (MoveInput == default) return;
        Vector3 movement = transform.right * MoveInput.x + transform.forward * MoveInput.y;
        _characterController.Move(movement * Speed * Time.deltaTime);
    }

    private void CheckIfGrounded()
    {
        if (IsGrounded)
        {
            IsGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _whatIsGround);
            if (!IsGrounded)
            {
                SoundManager.Instance.PlaySoundRandom(_jumpSounds);
            }
        }
        else
        {
            IsGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _whatIsGround);
            if (IsGrounded)
            {
                SoundManager.Instance.PlaySoundRandom(_landSounds);
            }
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

    private void ToggleAnimationState()
    {
        if (IsGrounded && MoveInput != default) _animator.SetBool("IsRunning", true);
        else _animator.SetBool("IsRunning", false);
    }

    private void OnEnable()
    {
        InputManager.MoveInputAction.started += WalkInputStart;
        InputManager.MoveInputAction.performed += WalkInput;
        InputManager.MoveInputAction.canceled += WalkInputStop;
        InputManager.JumpInputAction.performed += JumpInput;
    }
    private void OnDisable()
    {
        InputManager.MoveInputAction.started -= WalkInputStart;
        InputManager.MoveInputAction.performed -= WalkInput;
        InputManager.MoveInputAction.canceled -= WalkInputStop;
        InputManager.JumpInputAction.performed -= JumpInput;
    }
    private void WalkInputStart(InputAction.CallbackContext obj)
    {

    }
    private void WalkInput(InputAction.CallbackContext obj)
    {
        MoveInput = obj.ReadValue<Vector2>();
    }
    private void WalkInputStop(InputAction.CallbackContext obj)
    {
        MoveInput = default;
    }
    private void JumpInput(InputAction.CallbackContext obj)
    {
        if (IsGrounded) _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
    }
}
