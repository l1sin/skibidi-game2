using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _mouseSensitivityX;
    [SerializeField] private float _mouseSensitivityY;
    [SerializeField] private float _mobildeSensetivityMulty;
    private float _verticalRotation;
    private float _horizontalRotation;
    private Vector2 _rotation;
    

    private void OnEnable()
    {
        InputManager.LookInputAction.performed += Look;
        InputManager.LookInputAction.canceled += Look;
    }

    private void OnDisable()
    {
        InputManager.LookInputAction.performed -= Look;
        InputManager.LookInputAction.canceled -= Look;
    }

    private void Update()
    {
        if (!SaveManager.Instance.IsMobile) return;
        Rotate(_rotation);
    }

    private void Start()
    {
        CursorHelper.LockAndHideCursor();
        if (!SaveManager.Instance.IsMobile) return;
        _mouseSensitivityX *= _mobildeSensetivityMulty;
        _mouseSensitivityY *= _mobildeSensetivityMulty;
    }

    private void Look(InputAction.CallbackContext obj)
    {
        if (PauseManager.Paused) return;
        _rotation = obj.ReadValue<Vector2>();
        //Rotate(_rotation);
    }

    private void Rotate(Vector2 rotation)
    {
        _horizontalRotation = rotation.x * _mouseSensitivityX;
        _player.Rotate(Vector3.up * _horizontalRotation);

        _verticalRotation -= rotation.y * _mouseSensitivityY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }
}