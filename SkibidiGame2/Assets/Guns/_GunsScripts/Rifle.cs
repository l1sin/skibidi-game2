using UnityEngine;

public class Rifle : Gun
{
    protected override void Shoot()
    {
        TriggerShooting();
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