using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static readonly string HorizontalInput = "Horizontal";

    public static readonly string InteractInput = "Interact";

    public static readonly string JumpInput = "Jump";

    
    
    public static float GethorizontalInput()
    {
        return Input.GetAxis(HorizontalInput);
    }

    public static bool GetJInput()
    {
        return Input.GetButtonDown(InteractInput);
    }

    public static bool GetJumpInput()
    {
        return Input.GetButtonDown(JumpInput);
    }
}
