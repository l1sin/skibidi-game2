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
        AudioSource sound = SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);
        sound.volume = 0.7f;

        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            BeamHit(_tracer, hitInfo);
            SpawnImpact(hitInfo);
            MakeDamage(hitInfo);    
        }
        else
        {
            BeamMiss(_tracer);
        }
    }
}
