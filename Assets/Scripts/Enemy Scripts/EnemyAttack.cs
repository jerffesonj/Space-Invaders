using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : AttackBase
{
    void Start()
    {
        base.Start();
    }
    public void Shoot()
    {
        base.Shoot();
    }

    public void ShootRandomTime()
    {
        StartCoroutine(RandomShoot());
    }

    IEnumerator RandomShoot()
    {
        yield return new WaitForSeconds(Random.Range(1,5));
        base.Shoot();
    }
}
