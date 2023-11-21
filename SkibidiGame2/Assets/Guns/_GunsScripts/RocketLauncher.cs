using Sounds;
using UnityEngine;
using UnityEngine.InputSystem;

public class RocketLauncher : Gun
{
    [Header("Rocket Launcher")]
    [SerializeField] private GameObject _muzzleVFX;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private GameObject _rocket;
    [SerializeField] private float _launchDistanceFromCamera;

    [SerializeField] private float _rocketSpeed;
    [SerializeField] private float _rocketRadius;
    [SerializeField] private float _rocketLifeTime;

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
        Destroy(Instantiate(_muzzleVFX, _shootingPoint.position, transform.rotation), 5);
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);

        Instantiate(_rocket, _camera.transform.position + transform.TransformDirection(new Vector3(0,0, _launchDistanceFromCamera)) , transform.rotation);
        Rocket rocket = _rocket.GetComponent<Rocket>();
        rocket.Speed = _rocketSpeed;
        rocket.Radius = _rocketRadius;
        rocket.Damage = _damage;
        rocket.LifeTime = _rocketLifeTime;
        rocket.Targets = _targets;
        rocket.Decal = _decal;
        rocket.DecalLayerMask = _decalLayerMask;
        rocket.Impact = _impact;
    }
}
