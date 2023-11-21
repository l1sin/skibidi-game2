using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class GaussCannon : Gun
{
    [Header("Gauss Cannon")]
    [SerializeField] private ParticleSystem _shotVFX;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _gaussBeamVFX;
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

        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            // Beam effect.
            GameObject beamObj = Instantiate(_gaussBeamVFX, _shootingPoint.position, Quaternion.identity);
            beamObj.transform.LookAt(hitInfo.point);
            beamObj.transform.localScale = new Vector3(1, 1, hitInfo.distance);
            Destroy(beamObj, 5);

            SpawnImpact(hitInfo);
            SpawnDecal(hitInfo);
            MakeDamage(hitInfo);
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