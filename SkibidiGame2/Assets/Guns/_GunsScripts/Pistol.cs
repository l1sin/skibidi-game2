using Sounds;
using UnityEngine;

public class Pistol : GunTap
{
    public override void Shoot()
    {
        IsShooting = true;
        CanSwitch = false;
        Animator.SetTrigger("Shoot");
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

    public override void UpdateAmmo(float ammoConsumption)
    {
        Ammo -= ammoConsumption;
        WeaponController.UpdateAmmoText(9999);
    }
}