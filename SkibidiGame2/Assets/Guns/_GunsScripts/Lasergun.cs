using UnityEngine;
using UnityEngine.InputSystem;

public class Lasergun : Gun
{
    public GameObject LaserBeam;
    public LineRenderer Line;
    public AudioSource LaserSFX;
    public GameObject LaserImpactParticlesPrefab;
    private GameObject _laserImpactParticlesReference;
    public GameObject LaserMuzzleParticlesPrefab;
    private GameObject _laserMuzzleParticlesReference;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField][ColorUsage(false, true)] private Color _unchargedEmission;
    [SerializeField][ColorUsage(false, true)] private Color _chargedEmission;
    [SerializeField] private float _lerpValue = 0;
    [SerializeField] private bool _isLerping = false;


    protected override void Start()
    {
        _meshRenderer.materials[3].EnableKeyword("_EMISSION");
    }

    public void Update()
    {
        if (IsShootInput) Fire();
        if (_isLerping) LerpMaterial();
    }

    private void LerpMaterial()
    {
        Color color = Color.Lerp(_unchargedEmission, _chargedEmission, _lerpValue);
        _meshRenderer.materials[3].SetColor("_EmissionColor", color);
    }
    public override void OnShoot(InputAction.CallbackContext obj)
    {
        base.OnShoot(obj);
        if (!IsShooting) Shoot();
    }

    public override void OnEndShoot(InputAction.CallbackContext obj)
    {
        base.OnEndShoot(obj);
        EndShooting();
        LaserSFX.Stop();
        DestroyPartiles();
        LaserBeam.SetActive(false);
        Animator.SetBool("IsShooting", IsShooting);
        _isLerping = false;
        _meshRenderer.materials[3].SetColor("_EmissionColor", _unchargedEmission);
    }

    public void Shoot()
    {
        if (!IsShooting) LaserSFX.Play();
        IsShooting = true;
        CanSwitch = false;
        InstantiateParticles();
        LaserBeam.SetActive(true);
        Animator.SetBool("IsShooting", IsShooting);
    }

    public void Fire()
    {
        _laserMuzzleParticlesReference.transform.position = Line.transform.position;
        _laserMuzzleParticlesReference.transform.rotation = Line.transform.rotation;

        RaycastHit HitInfo;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out HitInfo, 100.0f, Targets))
        {
            Transform objectHit = HitInfo.transform;
            Line.SetPosition(1, Line.transform.InverseTransformPoint(HitInfo.point));
            _laserImpactParticlesReference.transform.position = HitInfo.point;

            IDamageable damageable = objectHit.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(Damage * Time.deltaTime);
            }
        }
        else
        {
            Vector3 newPosition = Camera.transform.position + Camera.transform.forward * 100f;
            Line.SetPosition(1, Line.transform.InverseTransformPoint(newPosition));
            _laserImpactParticlesReference.transform.position = newPosition;
        }
    }

    private void InstantiateParticles()
    {
        _laserImpactParticlesReference = Instantiate(LaserImpactParticlesPrefab);
        _laserMuzzleParticlesReference = Instantiate(LaserMuzzleParticlesPrefab);
    }

    private void MoveParticles()
    {

    }

    private void DestroyPartiles()
    {
        Destroy(_laserMuzzleParticlesReference, 5f);
        ParticleSystem ps1 = _laserMuzzleParticlesReference.GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule em1 = ps1.emission;
        em1.enabled = false;

        Destroy(_laserImpactParticlesReference, 5f);
        foreach (ParticleSystem ps2 in _laserImpactParticlesReference.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.EmissionModule em2 = ps2.emission;
            em2.enabled = false;
        }
    }

}
