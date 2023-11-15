using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _mouseSensitivityX;
    [SerializeField] private float _mouseSensitivityY;
    private float _verticalRotation;
    private float _horizontalRotation;

    private void OnEnable()
    {
        InputManager.LookInputAction.performed += Look;
    }

    private void OnDisable()
    {
        InputManager.LookInputAction.performed -= Look;
    }

    private void Start()
    {
        CursorHelper.LockAndHideCursor();
    }

    private void Look(InputAction.CallbackContext obj)
    {
        Vector2 rotation = obj.ReadValue<Vector2>();

        _horizontalRotation = rotation.x * _mouseSensitivityX;
        _player.Rotate(Vector3.up * _horizontalRotation);

        _verticalRotation -= rotation.y * _mouseSensitivityY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_verticalRotation, 0f,0f);
    }
}