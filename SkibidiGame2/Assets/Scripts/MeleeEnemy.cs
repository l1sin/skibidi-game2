using UnityEngine;

public class MeleeEnemy : Enemy
{
    public override void MakeDamage()
    {
        if (!_isDead)
        {
            Collider[] col = Physics.OverlapSphere(_attackCollider.position, _attackRadius, _attackLayerMask);
            if (col.Length > 0) col[0].GetComponent<IDamageable>().GetDamage(_damage);
        }
    }
}
