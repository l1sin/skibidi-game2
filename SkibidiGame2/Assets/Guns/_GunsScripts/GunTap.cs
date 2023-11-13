using UnityEngine;

public class GunTap : Gun
{
    public ParticleSystem ShotVFX;
    public AudioClip shotSound;
    public GameObject ImpactVFX;

    public void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !IsShooting)
        {
            Shoot();
        }

        Walk();
    }

    public virtual void Shoot() { }
}
