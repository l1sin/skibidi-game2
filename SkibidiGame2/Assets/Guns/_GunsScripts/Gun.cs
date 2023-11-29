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
    [SerializeField] protected GameObject _decal;
    [SerializeField] protected LayerMask _decalLayerMask;
    [SerializeField] protected GameObject _impact;
    [SerializeField] protected ParticleSystem _muzzle;
    [SerializeField] protected AudioClip _shotSound;
    [SerializeField] protected bool _shootingDelay;
    [SerializeField] protected Gun _secondGun;

    

    // System values.
    public float GunLevel;
    protected bool _isShooting = false;
    protected bool _isShootInput = false;
    [HideInInspector] public bool CanSwitch = true;
    protected static float s_maxShootingDistance = 100f;
    protected static float s_destroyEffectTime = 5f;

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
        if (!_isShooting && !_shootingDelay) Shoot();
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

    protected virtual void SpawnDecal(RaycastHit raycastHit)
    {
        if (_decalLayerMask == (_decalLayerMask | (1 << raycastHit.collider.gameObject.layer)))
        {
            GameObject decal = Instantiate(_decal, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
            Destroy(decal, s_destroyEffectTime);
        }
    }

    protected virtual void SpawnImpact(RaycastHit raycastHit)
    {
        GameObject particles = Instantiate(_impact, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
        Destroy(particles, s_destroyEffectTime);
    }

    protected virtual void MakeDamage(RaycastHit raycastHit)
    {
        IDamageable damageable = raycastHit.transform.GetComponentInParent<IDamageable>();
        if (damageable != null) damageable.GetDamage(_damage);
    }

    protected virtual void TriggerShooting()
    {
        _isShooting = true;
        CanSwitch = false;
        _animator.SetTrigger("Shoot");
        _muzzle.Play();
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);
    }

    protected virtual void Shoot() { }

    protected void BeamHit(GameObject beam, RaycastHit raycastHit)
    {
        GameObject beamObj = Instantiate(beam, _muzzle.transform.position, Quaternion.identity);
        beamObj.transform.LookAt(raycastHit.point);
        beamObj.transform.localScale = new Vector3(1, 1, raycastHit.distance);
        Destroy(beamObj, s_destroyEffectTime);
    }

    protected void BeamMiss(GameObject beam)
    {
        GameObject beamObj = Instantiate(beam, _muzzle.transform.position, Quaternion.identity);
        beamObj.transform.LookAt(_camera.transform.position + _camera.transform.forward * s_maxShootingDistance);
        beamObj.transform.localScale = new Vector3(1, 1, s_maxShootingDistance);
        Destroy(beamObj, s_destroyEffectTime);
    }

    public void TriggerSecondGun()
    {
        if (_secondGun != null && _secondGun.isActiveAndEnabled  && _isShootInput)
        {
            _secondGun.Shoot();
        }
    }
}
