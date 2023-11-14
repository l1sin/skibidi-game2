using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [HideInInspector] public static InputAction MoveInputAction;
        [HideInInspector] public static InputAction JumpInputAction;
        [HideInInspector] public static InputAction LookAction;
        [HideInInspector] public static InputAction Shoot;
        [HideInInspector] public static InputAction W1;
        [HideInInspector] public static InputAction W2;
        [HideInInspector] public static InputAction W3;
        [HideInInspector] public static InputAction W4;
        [HideInInspector] public static InputAction W5;
        [HideInInspector] public static InputAction W6;
        [HideInInspector] public static InputAction W7;
        [HideInInspector] public static InputAction W8;
        [HideInInspector] public static InputAction ScrollWeapon;

        private void Awake()
        {
            InputActionMap iam = _playerInput.currentActionMap;
            MoveInputAction = iam.FindAction("Move");
            JumpInputAction = iam.FindAction("Jump");
            LookAction = iam.FindAction("Look");
            Shoot = iam.FindAction("Shoot");
            W1 = iam.FindAction("1");
            W2 = iam.FindAction("2");
            W3 = iam.FindAction("3");
            W4 = iam.FindAction("4");
            W5 = iam.FindAction("5");
            W6 = iam.FindAction("6");
            W7 = iam.FindAction("7");
            W8 = iam.FindAction("8");
            ScrollWeapon = iam.FindAction("ScrollWeapon");
        }
    }
}
