using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Rocket : MonoBehaviour
{
    [Header("Public values")]
    public float Speed;
    public float Radius;
    public float Damage;
    public float LifeTime;
    public LayerMask Targets;
    public GameObject Decal;
    public LayerMask DecalLayerMask;
    public GameObject Impact;
    public AudioMixerGroup AudioMixerGroup;

    [Header("Private values")]
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _decalScale;
    [SerializeField] private AudioClip _explosionSFX;
    [SerializeField] private float _collisionRadius;

    private void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0) Explode();
        if (Physics.OverlapSphere(transform.position, _collisionRadius, Targets).Length > 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        _particleSystem.transform.parent = null;
        Destroy(_particleSystem.gameObject, 5f);
        ParticleSystem.EmissionModule em = _particleSystem.emission;
        em.enabled = false;

        AudioSource audio = SoundManager.Instance.PlaySound(_explosionSFX, AudioMixerGroup);
        audio.spatialBlend = 1;
        audio.minDistance = 10;
        audio.gameObject.transform.position = transform.position;

        SpawnImpact();

        Vector3 pos = transform.position - transform.forward;
        if (Physics.Raycast(pos, transform.forward, out RaycastHit hitInfo, 2, Targets))
        {
            SpawnDecal(hitInfo);
        }

        MakeDamage();
    }

    private void SpawnDecal(RaycastHit raycastHit)
    {

        if (DecalLayerMask == (DecalLayerMask | (1 << raycastHit.collider.gameObject.layer)))
        {
            GameObject decal = Instantiate(Decal, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
            decal.transform.localScale = Vector3.one * Radius * _decalScale;
            Destroy(decal, 5);
        }
    }

    private void SpawnImpact()
    {
        GameObject particles = Instantiate(Impact, transform.position, Quaternion.identity);
        Destroy(particles, 5);
    }

    private void MakeDamage()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, Radius, Targets);
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
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _collisionRadius);
    }
}
