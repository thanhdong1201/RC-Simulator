using UnityEngine;

public class JoystickPowerInput : JoystickBaseInput
{
    protected override void HandleInput(Vector2 input)
    {
        inputReader.OnPower(input); 
    }
}
