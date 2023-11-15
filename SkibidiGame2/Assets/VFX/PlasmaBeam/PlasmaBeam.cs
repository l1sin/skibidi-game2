using UnityEngine;

public class PlasmaBeam : MonoBehaviour
{
    public LineRenderer Line;
    public float ThinningPerSec;

    public void Update()
    {
        if (Line.widthMultiplier > 0)
            Line.widthMultiplier -= ThinningPerSec * Time.deltaTime;
    }
}
