using UnityEngine;

public class Rifle : Gun
{
    protected override void Shoot()
    {
        TriggerShooting();
        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            SpawnImpact(hitInfo);
            SpawnDecal(hitInfo);
            MakeDamage(hitInfo);
        }
    }
}