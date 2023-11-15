using UnityEngine;

public class Test : MonoBehaviour
{
    public Renderer _meshRenderer;
    [ColorUsage(false, true)] public Color _unchargedMaterial;
    [ColorUsage(false, true)] public Color _chargedMaterial;
    public float _lerpValue;

    private void Start()
    {
        _meshRenderer.material.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        LerpMaterial();
    }

    private void LerpMaterial()
    {
        Color color = Color.Lerp(_unchargedMaterial, _chargedMaterial, _lerpValue);
        _meshRenderer.material.SetColor("_EmissionColor", color);
    }
}
