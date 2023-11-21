using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minigun : Gun
{
    [Header("Minigun")]
    [SerializeField] private ParticleSystem _muzzleVFX;
    [SerializeField] private AudioClip _shotSound;

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

        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            SpawnImpact(hitInfo);
            SpawnDecal(hitInfo);
            MakeDamage(hitInfo);
        }
    }
}
