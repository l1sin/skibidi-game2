using UnityEngine;

public class GaussEmitParticles : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _lifeTime;

    private void Start()
    {
        _lifeTime = transform.parent.localScale.z / _speed;
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime > 0) MoveForward();
        else Destroy(gameObject);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
