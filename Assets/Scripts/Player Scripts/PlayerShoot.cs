using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : AttackBase
{
    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.Space))
        {
            base.Shoot();
        }
    }
}
