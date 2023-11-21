using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : Gun
{
    [Header("Shotgun")]
    [SerializeField] private ParticleSystem _muzzleVFX;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private int _bullets;
    [SerializeField] private float _maxDeviation;
    public override void OnShoot(InputAction.CallbackContext obj)
    {
        base.OnShoot(obj);
        if (!_isShooting) Shoot();
    }

    public void Shoot()
    {
        _isShooting = true;
        CanSwitch = false;
        _animator.SetTrigger("Shoot");
        _muzzleVFX.Play();
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);

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
                SpawnDecal(hitInfo);
                MakeDamage(hitInfo);
            }
        }
    }
}