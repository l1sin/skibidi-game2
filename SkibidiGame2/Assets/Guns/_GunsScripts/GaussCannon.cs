using UnityEngine;

public class GaussCannon : Gun
{
    [Header("Gauss Cannon")]
    [SerializeField] private GameObject _gaussBeamVFX;
    protected override void Shoot()
    {
        TriggerShooting();
        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            BeamHit(_gaussBeamVFX, hitInfo);
            SpawnImpact(hitInfo);
            MakeDamage(hitInfo);
        }
        else BeamMiss(_gaussBeamVFX);
    }
}