using UnityEngine;

public class EnemyRocket : MonoBehaviour
{
    public float Speed;
    public float Damage;
    public LayerMask TargetLayerMask;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _collisionRadius;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime < 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        Collider[] col = Physics.OverlapSphere(transform.position, _collisionRadius, TargetLayerMask);
        if (col.Length > 0)
        {
            col[0].gameObject.GetComponent<IDamageable>().GetDamage(Damage);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _collisionRadius);
    }

}
