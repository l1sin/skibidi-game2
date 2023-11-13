using Sounds;
using UnityEngine;

public class Minigun : GunHold
{
    public ParticleSystem ShotVFX;
    public AudioClip shotSound;
    public GameObject ImpactVFX;

    public override void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Ammo > 2)
            {
                IsShooting = true;
                CanSwitch = false;
                Animator.SetBool("IsShooting", IsShooting);
            }
            else
            {
                EndShooting();
                Animator.SetBool("IsShooting", IsShooting);
            }
        }
        else
        {
            Animator.SetBool("IsShooting", IsShooting);
        }
        Walk();
    }

    public void Fire()
    {
        UpdateAmmo(1);
        ShotVFX.Play();
        SoundManager.Instance.PlaySound(shotSound);

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
