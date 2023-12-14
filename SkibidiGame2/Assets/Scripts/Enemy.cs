using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public EnemySpawner EnemySpawner;
    [SerializeField] public Transform FollowTarget;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected float _speedCoef;
    [SerializeField] protected float _speedModificator;
    [SerializeField] protected GameObject _particles;
    [SerializeField] protected Transform _explosionPlace;
    [SerializeField] protected Transform _attackCollider;
    [SerializeField] protected float _attackRadius;
    [SerializeField] protected LayerMask _attackLayerMask;
    [SerializeField] protected float _damage;
    [SerializeField] protected bool _inAttackRange;

    [SerializeField] protected float _healthCurrent;
    [SerializeField] protected float _healthMax;
    [SerializeField] protected bool _isDead;
    [SerializeField] protected List<GameObject> _toDestroy;

    [SerializeField] protected float _healthBuff;
    [SerializeField] protected float _damageBuff;
    [SerializeField] protected float _speedBuff;

    [SerializeField] protected AudioClip _deathSound;
    [SerializeField] protected AudioClip _meatExplosion;
    [SerializeField] protected AudioMixerGroup _audioMixerGroup;

    private void Init()
    {
        _healthCurrent = _healthMax;
        _speedModificator = _agent.speed / _speedCoef;
        _animator.SetFloat("SpeedModificator", _speedModificator);

    }
    protected virtual void Update()
    {
        if (!_isDead)
        {
            _inAttackRange = Physics.CheckSphere(_attackCollider.position, _attackRadius, _attackLayerMask);
            _animator.SetBool("InAttackRange", _inAttackRange);
            _agent.destination = FollowTarget.position;
        }
    }

    public void SetTier(int tier, Color color)
    {
        SetColor(color);
        SetBuff(tier);
        Init();
    }

    private void SetBuff(int tier)
    {
        _healthMax *= Mathf.Pow(_healthBuff, tier);
        _damage *= Mathf.Pow(_damageBuff, tier);
        _agent.speed *= Mathf.Pow(_speedBuff, tier);
    }

    public void SetColor(Color color)
    {
        SkinnedMeshRenderer smr = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        foreach (Material m in smr.materials)
        {
            if (m.name == "Green (Instance)") m.color = color;
        }
    }

    public virtual void MakeDamage() { }

    public void Die()
    {
        EnemySpawner.IncrementDead();
        
    }

    public void DeathSound(AudioClip clip)
    {
        AudioSource audio = SoundManager.Instance.PlaySound(clip, _audioMixerGroup);
        audio.spatialBlend = 1;
        audio.minDistance = 10;
        audio.gameObject.transform.position = transform.position;
    }

    public void GetDamage(float damage)
    {
        if (!_isDead)
        {
            _healthCurrent -= damage;
            if (_healthCurrent <= 0)
            {
                SetDead();
                if (damage >= _healthMax)
                {
                    Destroy(Instantiate(_particles, _explosionPlace.position, transform.rotation), 5);
                    DeathSound(_meatExplosion);
                    Die();
                    Destroy(gameObject);
                }
                else
                {
                    _animator.SetTrigger("Death");
                    DeathSound(_deathSound);
                    Die();
                    Destroy(gameObject, 2);
                }
            }
        } 
    }

    private void SetDead()
    {
        _inAttackRange = false;
        _animator.SetBool("InAttackRange", _inAttackRange);
        _isDead = true;
        _agent.isStopped = true;
        foreach(GameObject c in _toDestroy)
        {
            Destroy(c);
        }
    }

    public void StopMovement()
    {
        _agent.isStopped = true;
    }

    public void ContinueMovement()
    {
        _agent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackCollider.position, _attackRadius);
    }
}
