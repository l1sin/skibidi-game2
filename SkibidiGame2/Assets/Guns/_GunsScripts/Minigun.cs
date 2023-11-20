using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minigun : Gun
{
    [Header("Minigun")]
    [SerializeField] private ParticleSystem _muzzleVFX;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _impactVFX;
    [SerializeField] private GameObject _decalVFX;

    public override void OnShoot(InputAction.CallbackContext obj)
    {
        base.OnShoot(obj);
        if (!_isShooting) Shoot();
    }

    public override void EndShooting()
    {
        base.EndShooting();
        _animator.SetBool("IsShooting", _isShooting);
    }

    public void Shoot()
    {
        _isShooting = true;
        CanSwitch = false;
        _animator.SetBool("IsShooting", _isShooting);
    }

    public void Fire()
    {
        _muzzleVFX.Play();
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);

        RaycastHit HitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out HitInfo, s_maxShootingDistance, _targets))
        {
            Transform objectHit = HitInfo.transform;
            GameObject particles = Instantiate(_impactVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            Destroy(particles, 5);

            GameObject decal = Instantiate(_decalVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            Destroy(decal, 5);

            IDamageable damageable = objectHit.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(_damage);
            }
        }
    }
}
