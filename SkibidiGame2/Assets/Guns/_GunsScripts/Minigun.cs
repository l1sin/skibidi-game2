using Sounds;
using UnityEngine;

public class Minigun : Gun
{
    public override void EndShooting()
    {
        base.EndShooting();
        _animator.SetBool("IsShooting", _isShooting);
    }

    protected override void Shoot()
    {
        _isShooting = true;
        CanSwitch = false;
        _animator.SetBool("IsShooting", _isShooting);
    }

    public void Fire()
    {
        _muzzle.Play();
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);

        RaycastHit HitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out HitInfo, s_maxShootingDistance, _targets))
        {
            SpawnImpact(HitInfo);
            MakeDamage(HitInfo);
        }
    }
}
