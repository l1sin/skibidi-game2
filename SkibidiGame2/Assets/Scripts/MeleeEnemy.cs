using UnityEngine;

public class MeleeEnemy : Enemy
{
    public override void MakeDamage()
    {
        Collider[] col = Physics.OverlapSphere(_attackCollider.position, _attackRadius, _attackLayerMask);
        if (col.Length > 0) col[0].GetComponent<IDamageable>().GetDamage(_damage);
    }
}
