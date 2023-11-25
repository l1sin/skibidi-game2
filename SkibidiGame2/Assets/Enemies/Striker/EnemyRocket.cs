using UnityEngine;

public class EnemyRocket : MonoBehaviour
{
    public float Speed;
    public float Damage;
    public LayerMask TargetLayerMask;
    [SerializeField] private float _lifeTime;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime < 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TargetLayerMask == (TargetLayerMask | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<IDamageable>().GetDamage(Damage);
            Destroy(gameObject);
        }
    }

}
