using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _orbitRadius;
    [SerializeField] private float _orbitSpeed;
    [SerializeField] private GameObject _movingObject;
    [SerializeField] private TrailRenderer _line;
    [SerializeField] private float _thinningPerSec;


    private void Update()
    {
        MoveForward();
        MoveSpiral();
        if (_line.widthMultiplier > 0) _line.widthMultiplier -= _thinningPerSec * Time.deltaTime;
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
    private void MoveSpiral()
    {
        float x = Mathf.Cos(Time.time * _orbitSpeed);
        float y = Mathf.Sin(Time.time * _orbitSpeed);
        _movingObject.transform.localPosition = new Vector3(x, y, 0) * _orbitRadius;
    }
}
