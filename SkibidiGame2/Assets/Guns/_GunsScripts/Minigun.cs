using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Minigun : Gun
{
    public ParticleSystem ShotVFX;
    public AudioClip shotSound;
    public GameObject ImpactVFX;

    public override void OnShoot(InputAction.CallbackContext obj)
    {
        base.OnShoot(obj);
        if (!IsShooting) Shoot();
    }

    public override void EndShooting()
    {
        base.EndShooting();
        Animator.SetBool("IsShooting", IsShooting);
    }

    public void Shoot()
    {
        IsShooting = true;
        CanSwitch = false;
        Animator.SetBool("IsShooting", IsShooting);
    }

    public void Fire()
    {
        ShotVFX.Play();
        SoundManager.Instance.PlaySound(shotSound, _audioMixerGroup);

        RaycastHit HitInfo;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out HitInfo, 100.0f, Targets))
        {
            Transform objectHit = HitInfo.transform;
            GameObject particles = Instantiate(ImpactVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            Destroy(particles, 5);

            IDamageable damageable = objectHit.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(Damage);
            }
        }
    }
}
