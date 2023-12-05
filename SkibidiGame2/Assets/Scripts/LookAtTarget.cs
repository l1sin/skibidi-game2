using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] public Transform Target;

    private void Update()
    {
        transform.LookAt(Target);
    }
}
