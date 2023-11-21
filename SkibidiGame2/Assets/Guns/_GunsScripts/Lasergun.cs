using UnityEngine;
using UnityEngine.InputSystem;

public class Lasergun : Gun
{
    [Header("Lasergun")]
    [SerializeField] private GameObject _laserBeam;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private AudioSource _laserSFX;
    [SerializeField] private GameObject _decalVFX;
    [SerializeField] private GameObject _laserMuzzleParticlesPrefab;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField][ColorUsage(false, true)] private Color _chargedEmission;
    [SerializeField] private float _lerpValue = 0;
    [SerializeField] private bool _isLerping = false;

    // System values.
    private GameObject _laserMuzzleParticlesReference;
    private GameObject _laserImpactParticlesReference;


    protected override void Start()
    {
        base.Start();
        _meshRenderer.materials[3].EnableKeyword("_EMISSION");
    }

    public void Update()
    {
        if (_isShootInput) Fire();
        if (_isLerping) LerpMaterial();
    }

    private void LerpMaterial()
    {
        Color color = Color.Lerp(Color.black, _chargedEmission, _lerpValue);
        _meshRenderer.materials[3].SetColor("_EmissionColor", color);
    }
    public override void OnShoot(InputAction.CallbackContext obj)
    {
        base.OnShoot(obj);
        if (!_isShooting) Shoot();
    }

    public override void OnEndShoot(InputAction.CallbackContext obj)
    {
        base.OnEndShoot(obj);
        EndShooting();
        _laserSFX.Stop();
        DestroyPartiles();
        _laserBeam.SetActive(false);
        _animator.SetBool("IsShooting", _isShooting);
        _isLerping = false;
        _meshRenderer.materials[3].SetColor("_EmissionColor", Color.black);
    }

    public void Shoot()
    {
        if (!_isShooting) _laserSFX.Play();
        _isShooting = true;
        CanSwitch = false;
        InstantiateParticles();
        _laserBeam.SetActive(true);
        _animator.SetBool("IsShooting", _isShooting);
    }

    public void Fire()
    {
        _laserMuzzleParticlesReference.transform.position = _line.transform.position;
        _laserMuzzleParticlesReference.transform.rotation = _line.transform.rotation;

        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            _line.SetPosition(1, _line.transform.InverseTransformPoint(hitInfo.point));
            _laserImpactParticlesReference.transform.position = hitInfo.point;

            SpawnDecal(hitInfo);
            MakeDamage(hitInfo);
        }
        else
        {
            Vector3 newPosition = _camera.transform.position + _camera.transform.forward * s_maxShootingDistance;
            _line.SetPosition(1, _line.transform.InverseTransformPoint(newPosition));
            _laserImpactParticlesReference.transform.position = newPosition;
        }
    }

    private void InstantiateParticles()
    {
        _laserImpactParticlesReference = Instantiate(_impact);
        _laserMuzzleParticlesReference = Instantiate(_laserMuzzleParticlesPrefab);
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
