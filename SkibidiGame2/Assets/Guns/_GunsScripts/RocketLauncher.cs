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

        GameObject newRocket = Instantiate(_rocket, _camera.transform.position + transform.TransformDirection(new Vector3(0, 0, _launchDistanceFromCamera)), transform.rotation);
        Rocket rocket = newRocket.GetComponent<Rocket>();
        rocket.Speed = _rocketSpeed;
        rocket.Radius = _rocketRadius;
        rocket.Damage = _damage;
        rocket.LifeTime = _rocketLifeTime;
        rocket.Targets = _targets;
        rocket.Impact = _impact;
        rocket.AudioMixerGroup = _audioMixerGroup;
    }
}
