using Sounds;
using UnityEngine.InputSystem;
using UnityEngine;

public class GaussCannon : Gun
{
    [Header("Gauss Cannon")]
    [SerializeField] private ParticleSystem _shotVFX;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _impactVFX;
    [SerializeField] private GameObject _gaussBeamVFX;
    [SerializeField] private GameObject _decalVFX;
    [SerializeField] private Transform _shootingPoint;

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
        _shotVFX.Play();
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);

        RaycastHit HitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out HitInfo, s_maxShootingDistance, _targets))
        {
            // Beam effect.
            GameObject beamObj = Instantiate(_gaussBeamVFX, _shootingPoint.position, Quaternion.identity);
            beamObj.transform.LookAt(HitInfo.point);
            beamObj.transform.localScale = new Vector3(1, 1, HitInfo.distance);
            Destroy(beamObj, 5);

            // Impact effect.
            Transform objectHit = HitInfo.transform;
            GameObject particles = Instantiate(_impactVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            Destroy(particles, 5);

            // Decal.
            GameObject decal = Instantiate(_decalVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            Destroy(decal, 5);

            // Damage target.
            IDamageable damageable = objectHit.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(_damage);
            }
        }
        else
        {
            GameObject beamObj = Instantiate(_gaussBeamVFX, _shootingPoint.position, Quaternion.identity);
            beamObj.transform.LookAt(_camera.transform.position + _camera.transform.forward * s_maxShootingDistance);
            beamObj.transform.localScale = new Vector3(1, 1, s_maxShootingDistance);
            Destroy(beamObj, 5);
        }
    }
}