using UnityEngine;

public class RocketLauncher : Gun
{
    [Header("Rocket Launcher")]
    [SerializeField] private GameObject _rocket;
    [SerializeField] private float _launchDistanceFromCamera;

    [SerializeField] private float _rocketSpeed;
    [SerializeField] private float _rocketRadius;
    [SerializeField] private float _rocketLifeTime;
    protected override void Shoot()
    {
        TriggerShooting();

        Instantiate(_rocket, _camera.transform.position + transform.TransformDirection(new Vector3(0, 0, _launchDistanceFromCamera)), transform.rotation);
        Rocket rocket = _rocket.GetComponent<Rocket>();
        rocket.Speed = _rocketSpeed;
        rocket.Radius = _rocketRadius;
        rocket.Damage = _damage;
        rocket.LifeTime = _rocketLifeTime;
        rocket.Targets = _targets;
        rocket.Decal = _decal;
        rocket.DecalLayerMask = _decalLayerMask;
        rocket.Impact = _impact;
        rocket.AudioMixerGroup = _audioMixerGroup;
    }
}
