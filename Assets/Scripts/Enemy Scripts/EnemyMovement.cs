using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MovementBase
{
    public float direction;

    void Update()
    {
        Move(direction);
    }
}
