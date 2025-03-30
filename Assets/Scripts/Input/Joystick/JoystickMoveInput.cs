using UnityEngine;

public class JoystickMoveInput : JoystickBaseInput
{
    protected override void HandleInput(Vector2 input)
    {
        inputReader.OnMove(input);
    }
}
