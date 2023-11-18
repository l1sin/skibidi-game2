using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalFade : MonoBehaviour
{
    [SerializeField] private DecalProjector _decalPojector;
    [SerializeField] private float _fadeSpeed;

    private void Update()
    {
        _decalPojector.fadeFactor -= Time.deltaTime * _fadeSpeed;
    }
}
