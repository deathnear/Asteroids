
using InputSystem;
using UnityEngine;

public class KeyboardInput : IInput
{
    public string Name { get; private set; }
    public InputType InputType { get; } = InputType.Keyboard;

    public KeyboardInput(string name)
    {
        Name = name;
    }

    public void Update(Player player)
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.Accelerate();
        }
        else
        {
            player.SlowDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Shoot();
        }

        if (Input.GetKey(KeyCode.A))
        {
            player.RotateAt(1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            player.RotateAt(-1);
        }
        
    }
}
