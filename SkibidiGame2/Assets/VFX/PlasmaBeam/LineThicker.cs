using UnityEngine;

public class LineThicker : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _thinningPerSec;

    public void Update()
    {
        if (_line.widthMultiplier > 0)
            _line.widthMultiplier -= _thinningPerSec * Time.deltaTime;
    }
}
