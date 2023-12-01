using UnityEngine;

public class Shotgun : Gun
{
    [Header("Shotgun")]
    [SerializeField] private int _bullets;
    [SerializeField] private float _maxDeviation;
    protected override void Shoot()
    {
        TriggerShooting();
        RaycastHit hitInfo;
        for (int i = 0; i < _bullets; i++)
        {
            Vector3 forwardVector = Vector3.forward;
            float deviation = Random.Range(0f, _maxDeviation);
            float angle = Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = _camera.transform.rotation * forwardVector;

            if (Physics.Raycast(_camera.transform.position, forwardVector, out hitInfo, s_maxShootingDistance, _targets))
            {
                SpawnImpact(hitInfo);
                MakeDamage(hitInfo);
            }
        }
    }
}