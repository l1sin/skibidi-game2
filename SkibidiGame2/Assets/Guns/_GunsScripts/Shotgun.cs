using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : Gun
{
    [Header("Shotgun")]
    [SerializeField] private ParticleSystem _muzzleVFX;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _impactVFX;
    [SerializeField] private GameObject _decalVFX;
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

        RaycastHit HitInfo;
        for (int i = 0; i < _bullets; i++)
        {
            Vector3 forwardVector = Vector3.forward;
            float deviation = Random.Range(0f, _maxDeviation);
            float angle = Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = _camera.transform.rotation * forwardVector;

            if (Physics.Raycast(_camera.transform.position, forwardVector, out HitInfo, s_maxShootingDistance, _targets))
            {
                Transform objectHit = HitInfo.transform;
                GameObject particles = Instantiate(_impactVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
                Destroy(particles, 5);

                GameObject decal = Instantiate(_decalVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
                Destroy(decal, 5);

                IDamageable damageable = objectHit.GetComponentInParent<IDamageable>();
                if (damageable != null)
                {
                    damageable.GetDamage(_damage);
                }
            }
        }
    }
}