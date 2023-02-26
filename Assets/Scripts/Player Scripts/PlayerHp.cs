using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : Hp
{
    public delegate void OnPlayerDeath();
    public event OnPlayerDeath onPlayerDeath;

    public override void RemoveHp(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            onPlayerDeath?.Invoke();
        }
    }
}
