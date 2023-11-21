using System.Collections.Generic;
using UnityEngine;

public class Plasmagun : Gun
{
    [Header("Plasmagun")]
    [SerializeField] private GameObject _plasmaBeamVFX;
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
    protected override void Shoot()
    {
        TriggerShooting();
        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            BeamHit(_plasmaBeamVFX, hitInfo);
            SpawnImpact(hitInfo);
            SpawnDecal(hitInfo);
            MakeDamage(hitInfo);
        }
        else BeamMiss(_plasmaBeamVFX);
    }

    protected override void SpawnDecal(RaycastHit raycastHit)
    {
        if (_decalLayerMask == (_decalLayerMask | (1 << raycastHit.collider.gameObject.layer)))
        {
            GameObject decal = Instantiate(_decal, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
            decal.transform.localScale = new Vector3(_radius, _radius, _radius);
            Destroy(decal, 5);
        }
    }

    protected override void SpawnImpact(RaycastHit raycastHit)
    {
        GameObject particles = Instantiate(_impact, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
        particles.transform.localScale = new Vector3(_radius, _radius, _radius);
        Destroy(particles, 5);
    }

    protected override void MakeDamage(RaycastHit raycastHit)
    {
        Collider[] targets = Physics.OverlapSphere(raycastHit.point, _radius, _targets);
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
}
