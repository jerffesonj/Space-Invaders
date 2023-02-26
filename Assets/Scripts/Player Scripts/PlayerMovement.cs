using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementBase
{
    private void Update()
    {
        Move(Input.GetAxis("Horizontal"));
    }
}
