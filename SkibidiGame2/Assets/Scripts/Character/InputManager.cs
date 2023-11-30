using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [HideInInspector] public static InputAction MoveInputAction;
        [HideInInspector] public static InputAction JumpInputAction;
        [HideInInspector] public static InputAction LookInputAction;
        [HideInInspector] public static InputAction ShootInputAction;
        [HideInInspector] public static InputAction[] ChangeWeaponInputAction =  new InputAction[8];
        [HideInInspector] public static InputAction ScrollWeaponInputAction;
        [HideInInspector] public static InputAction ScrollLeftInputAction;
        [HideInInspector] public static InputAction ScrollRightInputAction;

        private void Awake()
        {
            InputActionMap iam = _playerInput.currentActionMap;
            MoveInputAction = iam.FindAction("Move");
            JumpInputAction = iam.FindAction("Jump");
            LookInputAction = iam.FindAction("Look");
            ShootInputAction = iam.FindAction("Shoot");
            for (int i = 0; i < ChangeWeaponInputAction.Length; i++)
            {
                ChangeWeaponInputAction[i] = iam.FindAction((i+1).ToString());
            }
            ScrollWeaponInputAction = iam.FindAction("ScrollWeapon");
            ScrollLeftInputAction = iam.FindAction("ScrollLeft");
            ScrollRightInputAction = iam.FindAction("ScrollRight");
        }
    }
}
