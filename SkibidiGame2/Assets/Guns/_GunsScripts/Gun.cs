using Input;
using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected LayerMask _targets;
    [SerializeField] private float _animationSpeedModifyer;
    [SerializeField] protected AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioClip _weaponSwitchSound;
    [SerializeField] protected float _damage;
    [SerializeField] private float _damageBonus = 0.125f;
    [SerializeField] private float _animationSpeedBonus = 0.125f;

    // System values.
    public float GunLevel;
    protected bool _isShooting = false;
    protected bool _isShootInput = false;
    [HideInInspector] public bool CanSwitch = true;

    protected virtual void Start()
    {
        _animator.keepAnimatorStateOnDisable = true;
        _damage *= 1 + (GunLevel - 1) * _damageBonus;
        _animationSpeedModifyer *= 1 + (GunLevel - 1) * _animationSpeedBonus;
        _animator.SetFloat("AnimationSpeedModifyer", _animationSpeedModifyer);
    }

    public void WalkStart()
    {
        _animator.SetBool("IsShaking", true);
    }
    public void WalkEnd()
    {
        _animator.SetBool("IsShaking", false);
    }

    public void PlayTakeAnimation()
    {
        SoundManager.Instance.PlaySound(_weaponSwitchSound, _audioMixerGroup, 0.25f);
        _animator.SetTrigger("Take");
    }

    public virtual void EndShooting()
    {
        _isShooting = false;
        CanSwitch = true;
        if (_isShootInput) OnShoot(new InputAction.CallbackContext());
    }

    public virtual void OnShoot(InputAction.CallbackContext obj)
    {
        _isShootInput = true;
        _animator.ResetTrigger("Take");
    }

    public virtual void OnEndShoot(InputAction.CallbackContext obj) => _isShootInput = false;
    private void OnEnable()
    {
        InputManager.ShootInputAction.performed += OnShoot;
        InputManager.ShootInputAction.canceled += OnEndShoot;
    }

    private void OnDisable()
    {
        InputManager.ShootInputAction.performed -= OnShoot;
        InputManager.ShootInputAction.canceled -= OnEndShoot;
    }
}
