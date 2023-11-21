using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Rocket : MonoBehaviour
{
    [HideInInspector] public float Speed;
    [HideInInspector] public float Radius;
    [HideInInspector] public float Damage;
    [HideInInspector] public float LifeTime;
    [HideInInspector] public LayerMask Targets;
    [HideInInspector] public GameObject Decal;
    [HideInInspector] public LayerMask DecalLayerMask;
    [HideInInspector] public GameObject Impact;
    [HideInInspector] public AudioMixerGroup AudioMixerGroup;
    

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _decalScale;
    [SerializeField] private AudioClip _explosionSFX;

    private void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0) Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Targets == (Targets | (1 << other.gameObject.layer)))
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
}
