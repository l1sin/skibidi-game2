using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plasmagun : Gun
{
    public ParticleSystem ShotVFX;
    public AudioClip shotSound;
    public GameObject ImpactVFX;
    public float Radius;
    public GameObject PlasmaBeamVFX;
    public Transform ShootingPoint;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField][ColorUsage(false, true)] private Color _unchargedEmission;
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
        Color color = Color.Lerp(_unchargedEmission, _chargedEmission, _lerpValue);
        _meshRenderer.materials[2].SetColor("_EmissionColor", color);
    }

    public override void OnShoot(InputAction.CallbackContext obj)
    {
        base.OnShoot(obj);
        if (!IsShooting) Shoot();
    }

    public void Shoot()
    {
        IsShooting = true;
        CanSwitch = false;
        Animator.SetTrigger("Shoot");
        ShotVFX.Play();
        SoundManager.Instance.PlaySound(shotSound, _audioMixerGroup);

        RaycastHit HitInfo;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out HitInfo, 100.0f, Targets))
        {
            GameObject beamObj = Instantiate(PlasmaBeamVFX, ShootingPoint.position, Quaternion.identity);
            beamObj.transform.LookAt(HitInfo.point);
            beamObj.transform.localScale = new Vector3(1, 1, HitInfo.distance);
            Destroy(beamObj, 5);

            GameObject particles = Instantiate(ImpactVFX, HitInfo.point, Quaternion.LookRotation(HitInfo.normal));
            particles.transform.localScale = new Vector3(Radius, Radius, Radius);
            Destroy(particles, 5);

            Collider[] targets = Physics.OverlapSphere(HitInfo.point, Radius, Targets);
            HashSet<IDamageable> damageables = new HashSet<IDamageable>();

            foreach (Collider target in targets)
            {
                damageables.Add(target.GetComponentInParent<IDamageable>());
            }
            foreach (IDamageable damageable in damageables)
            {
                if (damageable != null)
                {
                    damageable.GetDamage(Damage);
                }
            }
        }
        else
        {
            GameObject beamObj = Instantiate(PlasmaBeamVFX, ShootingPoint.position, Quaternion.identity);
            beamObj.transform.LookAt(Camera.transform.position + Camera.transform.forward * 100f);
            beamObj.transform.localScale = new Vector3(1, 1, 100);
            Destroy(beamObj, 5);
        }
    }
}
