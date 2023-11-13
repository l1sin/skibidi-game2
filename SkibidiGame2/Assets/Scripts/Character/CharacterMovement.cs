using Sounds;
using UnityEngine;

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
    public bool IsGrounded = true;
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
        Jump();
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
        float x = CharacterInput.MoveInputX;
        float z = CharacterInput.MoveInputY;
        if ((x != 0 || z != 0) && IsGrounded)
        {
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
        Vector3 movement = (transform.right * x + transform.forward * z).normalized;
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

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
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
