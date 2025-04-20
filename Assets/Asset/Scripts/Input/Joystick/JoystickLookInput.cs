using UnityEngine;

public class JoystickLookInput : JoystickBaseInput
{
    protected override void HandleInput(Vector2 input)
    {
        inputReader.OnLook(input);
    }
}
