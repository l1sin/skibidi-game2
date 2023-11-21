using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _speedCoef;
    [SerializeField] private float _speedModificator;

    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;

    private void Start()
    {
        _healthCurrent = _healthMax;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void GetDamage(float damage)
    {
        _healthCurrent -= damage;
        if (_healthCurrent <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        _agent.destination = _followTarget.position;
        _speedModificator = _agent.speed / _speedCoef;
        _animator.SetFloat("SpeedModificator", _speedModificator);
    }
}
