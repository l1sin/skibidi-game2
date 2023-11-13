using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public static float MouseInputX;
    public static float MouseInputY;

    public static float MoveInputX;
    public static float MoveInputY;
    public static bool IsMoving;

    public static bool Jump;

    public bool InputOn = true;

    private void Update()
    {
        if (InputOn)
        {
            GetInput();
        }
        else
        {
            MouseInputX = 0;
            MouseInputY = 0;
            MoveInputX = 0;
            MoveInputY = 0;
        }
    }

    private void FixedUpdate()
    {
        Jump = false;
    }

    private void GetInput()
    {
        if (Input.GetButtonDown(GlobalStrings.JumpInput))
        {
            Jump = true;
        }

        MouseInputX = Input.GetAxis(GlobalStrings.MouseXInput);
        MouseInputY = Input.GetAxis(GlobalStrings.MouseYInput);

        MoveInputX = Input.GetAxisRaw(GlobalStrings.HorizontalInput);
        MoveInputY = Input.GetAxisRaw(GlobalStrings.VerticalInput);
        if (MoveInputX != 0 || MoveInputY != 0) IsMoving = true;
        else IsMoving = false;
    }
}
