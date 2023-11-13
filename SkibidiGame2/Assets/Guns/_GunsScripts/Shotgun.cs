using Sounds;
using UnityEngine;

public class Shotgun : GunTap
{
    public int Bullets;
    public float maxDeviation;
    public override void Shoot()
    {
        if (Ammo > 0)
        {
            UpdateAmmo(1);
            IsShooting = true;
            CanSwitch = false;
            Animator.SetTrigger("Shoot");
            ShotVFX.Play();
            SoundManager.Instance.PlaySound(shotSound);

            RaycastHit HitInfo;
            for (int i = 0; i < Bullets; i++)
            {
                Vector3 forwardVector = Vector3.forward;
                float deviation = Random.Range(0f, maxDeviation);
                float angle = Random.Range(0f, 360f);
                forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
                forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
                forwardVector = Camera.transform.rotation * forwardVector;

                if (Physics.Raycast(Camera.transform.position, forwardVector, out HitInfo, 100.0f, Targets))
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
        else
        {
            EndShooting();
        }
    }
}