using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plasmagun : Gun
{
    [Header("Plasmagun")]
    [SerializeField] private ParticleSystem _shotVFX;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _impactVFX;
    [SerializeField] private GameObject _plasmaBeamVFX;
    [SerializeField] private GameObject _decalVFX;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _radius;
    [SerializeField][ColorUsage(false, true)] private Color _chargedEmission;
    [SerializeField] private float _lerpValue = 0;
    [SerializeField] private bool _isLerping = false;


    protected override void Start()
    {
        base.Start();
        _meshRenderer.materials[2].EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        if (_isLerping) LerpMaterial();
    }
    private void LerpMaterial()
    {
        Color color = Color.Lerp(Color.black, _chargedEmission, _lerpValue);
        _meshRenderer.materials[2].SetColor("_EmissionColor", color);
    }

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
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out HitInfo, 100.0f, _targets))
        {
            GameObject beamObj = Instantiate(_plasmaBeamVFX, _shootingPoint.position, Quaternion.identity);
            beamObj.transform.LookAt(HitInfo.point);
            beamObj.transform.localScale = new Vector3(1, 1, HitInfo.distance);
            Destroy(beamObj, 5);

            GameObject particles = Instantiate(_impactVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            particles.transform.localScale = new Vector3(_radius, _radius, _radius);
            Destroy(particles, 5);

            GameObject decal = Instantiate(_decalVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            decal.transform.localScale = new Vector3(_radius, _radius, _radius);
            Destroy(decal, 5);

            Collider[] targets = Physics.OverlapSphere(HitInfo.point, _radius, _targets);
            HashSet<IDamageable> damageables = new HashSet<IDamageable>();

            foreach (Collider target in targets)
            {
                damageables.Add(target.GetComponentInParent<IDamageable>());
            }
            foreach (IDamageable damageable in damageables)
            {
                if (damageable != null)
                {
                    damageable.GetDamage(_damage);
                }
            }
        }
        else
        {
            GameObject beamObj = Instantiate(_plasmaBeamVFX, _shootingPoint.position, Quaternion.identity);
            beamObj.transform.LookAt(_camera.transform.position + _camera.transform.forward * 100f);
            beamObj.transform.localScale = new Vector3(1, 1, 100);
            Destroy(beamObj, 5);
        }
    }
}
