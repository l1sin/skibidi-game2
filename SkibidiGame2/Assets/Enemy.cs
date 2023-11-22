using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _speedCoef;
    [SerializeField] private float _speedModificator;
    [SerializeField] private GameObject _particles;

    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;
    [SerializeField] private bool _isDead;
    [SerializeField] private List<GameObject> _toDestroy;

    private void Start()
    {
        _healthCurrent = _healthMax;
        _speedModificator = _agent.speed / _speedCoef;
        _animator.SetFloat("SpeedModificator", _speedModificator);
    }

    public void Die()
    {
        Destroy(gameObject);
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
                    Destroy(Instantiate(_particles, transform.position, transform.rotation), 5);
                    Destroy(gameObject);
                }
                else
                {
                    _animator.SetTrigger("Death");
                    Destroy(gameObject, 2);
                }
            }
        } 
    }

    private void SetDead()
    {
        _isDead = true;
        _agent.isStopped = true;
        foreach(GameObject c in _toDestroy)
        {
            Destroy(c);
        }
    }

    private void Update()
    {
       if (!_isDead) _agent.destination = _followTarget.position;  
    }

}
