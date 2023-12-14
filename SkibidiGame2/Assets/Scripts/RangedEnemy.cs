using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform[] _shootingPoints;
    [SerializeField] private float _rocketSpeed;

    protected override void Update()
    {
        if (!_isDead)
        {
            _inAttackRange = Physics.CheckSphere(_attackCollider.position, _attackRadius, _attackLayerMask);
            _animator.SetBool("InAttackRange", _inAttackRange);
            _agent.destination = FollowTarget.position;
            if (_inAttackRange)
            {
                Vector3 targetPostition = new Vector3(FollowTarget.position.x, transform.position.y, FollowTarget.position.z);
                transform.LookAt(targetPostition);
            }
        }
    }

    public override void MakeDamage()
    {
        if (!_isDead)
        {
            Collider[] col = Physics.OverlapSphere(_attackCollider.position, _attackRadius, _attackLayerMask);
            if (col.Length > 0)
            {
                foreach (Transform t in _shootingPoints)
                {
                    GameObject bullet = Instantiate(_bullet, t.position, Quaternion.identity);
                    bullet.transform.LookAt(FollowTarget);
                    EnemyRocket rocket = bullet.GetComponent<EnemyRocket>();

                    rocket.Speed = _rocketSpeed;
                    rocket.Damage = _damage;
                    rocket.TargetLayerMask = _attackLayerMask;
                }
            }
        }  
    }
}
