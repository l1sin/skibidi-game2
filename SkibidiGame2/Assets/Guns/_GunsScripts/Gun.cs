using Input;
using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public WeaponController WeaponController;
    public CharacterMovement CharacterMovement;
    public Animator Animator;
    public Transform GunObject;
    public Camera Camera;
    public LayerMask Targets;
    public bool CanSwitch = true;
    public bool IsShooting = false;
    public float Damage;
    public float GunLevel;
    public float AnimationSpeedModifyer;

    public bool IsShootInput = false;

    [SerializeField] protected AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioClip _weaponSwitchSound;

    public float DamageBonus = 0.125f;
    public float AnimationSpeedBonus = 0.125f;

    protected virtual void Start()
    {
        Animator.keepAnimatorStateOnDisable = true;
        Damage *= 1 + (GunLevel - 1) * DamageBonus;
        AnimationSpeedModifyer *= 1 + (GunLevel - 1) * AnimationSpeedBonus;
        Animator.SetFloat("AnimationSpeedModifyer", AnimationSpeedModifyer);
    }

    public void WalkStart()
    {
        Animator.SetBool("IsShaking", true);
    }
    public void WalkEnd()
    {
        Animator.SetBool("IsShaking", false);
    }

    public void PlayTakeAnimation()
    {
        SoundManager.Instance.PlaySound(_weaponSwitchSound, _audioMixerGroup, 0.25f);
        Animator.SetTrigger("Take");
    }

    public virtual void EndShooting()
    {
        IsShooting = false;
        CanSwitch = true;
        if (IsShootInput) OnShoot(new InputAction.CallbackContext());
    }

    public virtual void OnShoot(InputAction.CallbackContext obj) => IsShootInput = true;
    public virtual void OnEndShoot(InputAction.CallbackContext obj) => IsShootInput = false;
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
